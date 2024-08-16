using System.Text.Json;
using PersonService.Dtos.Person;

namespace PersonService.Utilities
{
    public static class PersonUtils
    {
        public static List<PersonChangedValue> GetChangedValues(string? oldValue, string? newValue, List<string>? observableParameters)
        {
            var list = new List<PersonChangedValue>();
            try
            {
                //insert
                if (string.IsNullOrWhiteSpace(oldValue) && !string.IsNullOrWhiteSpace(newValue))
                {
                    CreateInsertChanges(newValue, observableParameters, list);
                }
                //update
                else if (!string.IsNullOrWhiteSpace(oldValue) && !string.IsNullOrWhiteSpace(newValue))
                {
                    CreateUpdateChanges(oldValue, newValue, observableParameters, list);
                }
                //delete
                else if (!string.IsNullOrWhiteSpace(oldValue) && string.IsNullOrWhiteSpace(newValue))
                {
                    CreateDeleteChanges(oldValue, observableParameters, list);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing changes: " + e.Message + e.StackTrace);
                return default;
            }
            return list;
        }



        private static void CreateInsertChanges(string newValue, List<string>? observableParameters, List<PersonChangedValue> changedValues)
        {
            var newObject = JsonSerializer.Deserialize<Dictionary<string, object>>(newValue); //use dictionary, because this json cannot be with nested objects, because it is representation of table
            if (newObject == null)
            {                
                return;
            }
            foreach (var field in newObject)
            {
                if (observableParameters == null || !observableParameters.Any())
                {
                    //if parameters are not specified, return all changes
                    changedValues.Add(new PersonChangedValue(field.Key, null, field.Value.ToString()));
                }
                else if (observableParameters.Exists(x => x.Equals(field.Key.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    changedValues.Add(new PersonChangedValue(field.Key, null, field.Value.ToString()));
                }
            }
        }
        private static void CreateUpdateChanges(string oldValue, string newValue, List<string>? observableParameters, List<PersonChangedValue> changedValues)
        {
            var oldObject = JsonSerializer.Deserialize<Dictionary<string, object>>(oldValue); //use dictionary, because this json cannot be with nested objects, because it is representation of table
            var newObject = JsonSerializer.Deserialize<Dictionary<string, object>>(newValue);

            if (newObject == null || oldObject == null)
            {
                return;
            }

            for (int i = 0; i < newObject.Keys.Count; i++)
            {
                var oldField = oldObject.ElementAt(i);
                var newField = newObject.ElementAt(i);

                if (oldField.Value.ToString().Equals(newField.Value.ToString())) 
                {
                    continue;
                }

                if (observableParameters == null || !observableParameters.Any())
                {
                    //if parameters are not specified, return all changes
                    changedValues.Add(new PersonChangedValue(newField.Key, oldField.Value.ToString(), newField.Value.ToString()));
                }
                else if (observableParameters.Exists(x => x.Equals(newField.Key.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    changedValues.Add(new PersonChangedValue(newField.Key, oldField.Value.ToString(), newField.Value.ToString()));
                }

            }
        }


        private static void CreateDeleteChanges(string oldValue, List<string>? observableParameters, List<PersonChangedValue> changedValues)
        {
            var oldObject = JsonSerializer.Deserialize<Dictionary<string, object>>(oldValue); //use dictionary, because this json cannot be with nested objects, because it is representation of table
            if (oldObject == null)
            {
                return;
            }
            foreach (var field in oldObject)
            {

                if (observableParameters == null || !observableParameters.Any())
                {
                    //if parameters are not specified, return all changes
                    changedValues.Add(new PersonChangedValue(field.Key, field.Value.ToString(), null));
                }
                else if (observableParameters.Exists(x => x.Equals(field.Key.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    changedValues.Add(new PersonChangedValue(field.Key, field.Value.ToString(), null));
                }
            }
        }
    }
}
