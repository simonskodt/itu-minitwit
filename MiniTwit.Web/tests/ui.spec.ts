import { test, expect } from '@playwright/test';

test('MiniTwit title', async ({ page }) => {
  await page.goto('http://::1:3000/public');

  await expect(page).toHaveTitle('Public | MiniTwit');
});


test('Register user via GUI', async ({ page }) => {
  await page.goto('http://::1:3000/register');

  //create randomUsername, because database fails if not a unique username
  const randomName = Math.random().toString(36).slice(2, 7);
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

  // add a delay of 2 seconds to allow time for the page to load
  await page.waitForTimeout(2000);

  await expect(page).toHaveURL("http://::1:3000/login");
});


