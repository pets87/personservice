import http from 'k6/http';
import { sleep } from 'k6';
import { check } from "k6";

export let options = {
  insecureSkipTLSVerify: true,
  noConnectionReuse: true,
  vus: 100,
  iterations: 500,
  duration: '1m'
};

//export let options = {
//  insecureSkipTLSVerify: true,
//  noConnectionReuse: true,
//  stages:[
//    {duration:'1m', target: 10},
//    {duration:'1m', target: 20},
//    {duration:'1m', target: 50},
//    {duration:'1m', target: 100}
//  ]
//};

let baseUrl = '<add url>';
//const baseUrl = 'http://host.docker.internal:8080';


const httpHeaders = {
  'Content-Type': 'application/json',
  'X-Road-UserId': "11111111111",
  'X-Road-client': 'rr/GOV/123456/personservice'
};

const firstNames = [ 'Alice', 'Bob', 'Charlie', 'David', 'Eve', 'Frank', 'Grace', 'Hannah', 'Ian', 'Jack', 'Karen', 'Leo', 'Mia', 'Noah', 'Olivia', 'Paul', 'Quinn', 'Rachel', 'Steve', 'Tina' ];
const lastNames = [ 'Smith', 'Johnson', 'Williams', 'Jones', 'Brown', 'Davis', 'Miller', 'Wilson', 'Moore', 'Taylor', 'Anderson', 'Thomas', 'Jackson', 'White', 'Harris', 'Martin', 'Thompson', 'Garcia', 'Martinez', 'Robinson' ];
const personCodes = [
  '83048293049', '23904820384', '48203948203', '23984028492', '49203849203',
  '20394820384', '48203948294', '92038492038', '38492038492', '49023849203',
  '92840382038', '38492038492', '20394820394', '49283048203', '38492038494',
  '92038492040', '38492038430', '49203948293', '20394829038', '92038492039'
];
const birthdays = [
  '1990-01-15T08:30:00Z', '1985-03-22T14:45:00Z', '1978-07-09T21:05:00Z', '2000-11-30T06:25:00Z', '1989-05-16T18:00:00Z',
  '1992-02-07T13:15:00Z', '1975-09-11T07:40:00Z', '1982-12-24T20:55:00Z', '1996-06-19T10:10:00Z', '2003-08-30T15:35:00Z',
  '1987-04-04T22:50:00Z', '1991-10-23T09:20:00Z', '1979-12-31T17:05:00Z', '1988-11-11T11:25:00Z', '1993-07-17T04:30:00Z',
  '2001-05-05T23:55:00Z', '1983-03-09T12:45:00Z', '1995-08-22T19:00:00Z', '2002-01-14T16:30:00Z', '1980-06-18T05:40:00Z'
];

function getRandomString(array) {
  if (!Array.isArray(array) || array.length === 0) {
    throw new Error('The input must be a non-empty array.');
  }

  const randomIndex = Math.floor(Math.random() * array.length);
  return array[ randomIndex ];
}



function createAndChangeRandomPerson() {
  let person = {
    'personalCode': getRandomString(personCodes),
    'firstName': getRandomString(firstNames),
    'lastName': getRandomString(lastNames),
    'dateOfBirth': getRandomString(birthdays)
  }


  const params = {
    headers: httpHeaders
  };
  const payload = JSON.stringify(person);
  const resp = http.post(baseUrl + '/api/person', payload, params);
  check(resp, {
    'is status 201': (r) => r.status === 201,
  });

  person = resp.json();
  if (person)
    changePerson(person);
}


function changePerson(person) {
  person.firstName += " CHANGED";

  const params = {
    headers: httpHeaders
  };
  const payload = JSON.stringify(person);
  const resp = http.put(baseUrl + '/api/person', payload, params);
  check(resp, {
    'is status 200': (r) => r.status === 200,
  });  
}


function getChanges() {
  const dateEnd = new Date();
  const startDate = new Date();
  startDate.setSeconds(startDate.getSeconds() - 2);
  let req = {
    'startTime': startDate ,
    'endTime': dateEnd,
    'observableParameters': ["FirstName"]
  };


  const params = {
    headers: httpHeaders
  };
  const payload = JSON.stringify(req);
  const resp = http.post(baseUrl + '/changes', payload, params);
  check(resp, {
    'is status 200': (r) => r.status === 200,
  });
}



export default () => {
  const sleepTime = Math.random() * 3;
  sleep(sleepTime);
  createAndChangeRandomPerson();
  getChanges();
}
