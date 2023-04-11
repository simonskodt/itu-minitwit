import { test, expect, Page } from '@playwright/test';
import { MongoClient,} from 'mongodb';
import * as fs from 'fs'
import { BrowserContext } from 'playwright';

async function findUserByUserName(username: string) { 
  const connectionStringFromFile = fs.readFileSync('../../.local/connection_string.txt','utf8');
  const str = connectionStringFromFile.substring(0, connectionStringFromFile.length-13); 

  const client = await MongoClient.connect(str +'localhost:27018');
  const db = client.db('MiniTwit');
  const collection = db.collection('Users');

  const query = { Username: username };
  const qu = await collection.findOne(query);
  return qu
}


test.describe.parallel('open session from dashboard', () => {
  test.slow();
  let randomName: string;
  let context: BrowserContext;
  let page: Page;

  test.beforeEach(async ({ browser }) => {
    test.slow()
    // Create a new browser context and page for each test
    context = await browser.newContext();
    page = await context.newPage();

    // Start the test logic
    await page.goto('http://localhost:3000/register');

    // create randomUsername, because database fails if not a unique username
    randomName = Math.random().toString(36).slice(2, 7);
    const inputElements = await page.$$('input');

    await inputElements[0].click();
    await page.keyboard.type("UiTest"+randomName);

    await inputElements[1].click();
    await page.keyboard.type(randomName+'@itu.dk');

    await inputElements[2].click();

    await page.keyboard.type('123');

    await inputElements[3].click();
    await page.keyboard.type('123');

    await page.click('input[type="submit"][value="Sign Up"]');
  });

  test('test_register_user_via_gui_and_check_db_entry', async () => {
    test.slow()
    // wait for the user to be created before checking the database
    await new Promise(resolve => setTimeout(resolve, 2000));

    const result = await findUserByUserName("UiTest"+randomName);

    expect(result?.Username).toBe("UiTest"+randomName);
  });

  test.afterEach(async () => {
    // Close the browser context and page after each test
    await page.close();
    await context.close();
  });
});


