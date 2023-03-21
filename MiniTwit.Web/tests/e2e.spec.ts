//import { test, expect  } from '@playwright/test';


//bot test
/* test.describe('register user via GUI and check DB entry', () => {
  const DB_URL = '<your-db-url-here>';
  let dbClient: MongoClient;
  let page: any;

  test.beforeAll(async () => {
    dbClient = await MongoClient.connect(DB_URL, { serverSelectionTimeoutMS: 5000 });
    // Ensure test is idempotent by cleaning up any previous test entries
    await dbClient.db('test').collection('user').deleteOne({ username: 'Me' });
  });

  test.afterAll(async () => {
    await dbClient.close();
  });

  test('should register user and check DB entry', async () => {
    // Ensure no such user exists in the database yet
    const existingUser = await _get_user_by_name(dbClient, 'Me');
    test.expect(existingUser).toBeNull();

    const browser = await firefox.launch();
    const context = await browser.newContext();
    page = await context.newPage();

    await page.goto('<your-app-url-here>');

    const nameInput = await page.$('#name');
    const emailInput = await page.$('#email');
    const passwordInput = await page.$('#password');
    const confirmPasswordInput = await page.$('#confirm-password');
    const submitButton = await page.$('#register-button');

    await nameInput.fill('Me');
    await emailInput.fill('me@some.where');
    await passwordInput.fill('secure123');
    await confirmPasswordInput.fill('secure123');

    await submitButton.click();

    const successMessage = await page.waitForSelector('#success-message');
    const generatedMsg = await successMessage.textContent();
    const expectedMsg = 'You were successfully registered and can login now';
    test.expect(generatedMsg).toBe(expectedMsg);

    const user = await _get_user_by_name(dbClient, 'Me');
    test.expect(user).not.toBeNull();
    test.expect(user!.username).toBe('Me');
  });

  async function _get_user_by_name(client: MongoClient, username: string) {
    const user = await client.db('test').collection('user').findOne({ username });
    return user;
  }
}); */