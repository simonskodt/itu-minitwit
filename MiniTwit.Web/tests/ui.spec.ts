import { test, expect } from '@playwright/test';

test('MiniTwit title', async ({ page }) => {
  await page.goto('http://164.92.167.188:3000/public');

  await expect(page).toHaveTitle('MiniTwit');
});


test('Register user via GUI', async ({ page }) => {
  await page.goto('http://164.92.167.188:3000/register');

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

  await expect(page).toHaveURL("http://164.92.167.188:3000/public");
});
