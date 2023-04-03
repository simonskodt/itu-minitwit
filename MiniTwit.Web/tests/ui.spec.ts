import { test, expect } from '@playwright/test';


test('MiniTwit title', async ({ page }) => {
  await page.goto('http://localhost:3000/public');

  await expect(page).toHaveTitle('MiniTwit');
});


test('Register user via GUI', async ({ page }) => {
  page.goto('http://localhost:3000/register');

  //create randomUsername, because databse fails if not a unique username
  const randomName = Math.random().toString(36).slice(2, 7);
  const inputElements =  page.$$('input');

  inputElements[0].click();
  page.keyboard.type("UiTest"+randomName);

  inputElements[1].click();
  page.keyboard.type(randomName+'@itu.dk');

  inputElements[2].click();
  page.keyboard.type('123');

  inputElements[3].click();
  page.keyboard.type('123');

  page.click('button:text("Sign Up")');

  expect(page).toHaveURL("http://localhost:3000/public");
});


