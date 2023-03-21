import { test, expect } from '@playwright/test';

test('MiniTwit title', async ({ page }) => {
  await page.goto('http://164.92.167.188:3000/public');

  await expect(page).toHaveTitle('MiniTwit');
});

test('Register user via ', async ({ page }) => {
  await page.goto('https://playwright.dev/');

  // Click the get started link.
  await page.getByRole('link', { name: 'Get started' }).click();

  // Expects the URL to contain intro.
  await expect(page).toHaveURL(/.*intro/);
});
