import { test, expect } from '@playwright/test';
import { MongoClient, Db, Collection, ObjectId } from 'mongodb';
import * as fs from 'fs'

test('MiniTwit title', async ({ page }) => {
  await page.goto('http://localhost:3000/public');

  await expect(page).toHaveTitle('MiniTwit');
});

test('Register user via GUI', async ({ page }) => {
  await page.goto('http://localhost:3000/register');

  //create randomUsername, because databse fails if not a unique username
  const randomName = Math.random().toString(36).slice(2, 7);
  const inputElements = await page.$$('input');

  const userName = await inputElements[0].click();
  await page.keyboard.type("UiTest"+randomName);

  const email = await inputElements[1].click();
  await page.keyboard.type(randomName+'@itu.dk');

  const password = await inputElements[2].click();
  await page.keyboard.type('123');

  const passwordRepeat = await inputElements[3].click();
  await page.keyboard.type('123');

  await page.click('button:text("Sign Up")');

  await expect(page).toHaveURL("http://localhost:3000/public");
});

async function findUserByUserName(username: string) { 
  let connectionStringFromFile = fs.readFileSync('../../.local/connection_string.txt','utf8');
  let str = connectionStringFromFile.substring(0, connectionStringFromFile.length-13); 

  const client = await MongoClient.connect(str +'localhost:27018');
  const db = client.db('MiniTwit');
  const collection = db.collection('Users');

  const query = { Username: username };
  const qu = await collection.findOne(query);
  return qu
}

test('test_register_user_via_gui_and_check_db_entry', async ({ page }) => {
  await page.goto('http://localhost:3000/register');

  // create randomUsername, because database fails if not a unique username
  const randomName = Math.random().toString(36).slice(2, 7);
  const inputElements = await page.$$('input');

  const userName = await inputElements[0].click();
  await page.keyboard.type("UiTest"+randomName);

  const email = await inputElements[1].click();
  await page.keyboard.type(randomName+'@itu.dk');

  const password = await inputElements[2].click();
  await page.keyboard.type('123');

  const passwordRepeat = await inputElements[3].click();
  await page.keyboard.type('123');

  await page.click('button:text("Sign Up")');

  // wait for the user to be created before checking the database
  await new Promise(resolve => setTimeout(resolve, 1000));

  const result = await findUserByUserName("UiTest"+randomName);

  expect(result?.Username).toBe("UiTest"+randomName);
});