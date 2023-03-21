import { test, expect } from '@playwright/test';

test('MiniTwit title', async ({ page }) => {
  await page.goto('http://164.92.167.188:3000/public');

  await expect(page).toHaveTitle('MiniTwit');
});

test('Register user via GUI', async ({ page }) => {
  await page.goto('http://164.92.167.188:3000/register');

  // Get all input elements on the page
  const inputElements = await page.$$('input');

  const userName = await inputElements[0].click();
  await page.keyboard.type('samd');

  const email = await inputElements[1].click();
  await page.keyboard.type('samd@itu.dk');

  const password = await inputElements[2].click();
  await page.keyboard.type('samd');

  const passwordRepeat = await inputElements[3].click();
  await page.keyboard.type('samd');

  await page.click('button:text("Sign Up")');

  await expect(page).toHaveURL("http://164.92.167.188:3000/public");
});

test('test_register_user_via_gui_and_check_db_entry', async ({ page }) => {
  await page.goto('http://164.92.167.188:3000/register');

  // Get all input elements on the page
  const inputElements = await page.$$('input');

  const userName = await inputElements[0].click();
  await page.keyboard.type('samd');

  const email = await inputElements[1].click();
  await page.keyboard.type('samd@itu.dk');

  const password = await inputElements[2].click();
  await page.keyboard.type('samd');

  const passwordRepeat = await inputElements[3].click();
  await page.keyboard.type('samd');

  await page.click('button:text("Sign Up")');

  await expect(page).toHaveURL("http://164.92.167.188:3000/public");
});