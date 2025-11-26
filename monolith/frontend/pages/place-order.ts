// UI Controller for Place Order page

import { showNotification, handleResult, showSuccessNotification } from '../common';
import { orderService } from '../services/order-service';
import type { OrderFormData } from '../types/form.types';

document.getElementById('orderForm')?.addEventListener('submit', async function(e: Event) {
  e.preventDefault();

  const orderData = collectFormData();

  if (!validateFormData(orderData)) {
    return;
  }

  const result = await orderService.placeOrder(orderData.sku, orderData.quantity, orderData.country);

  handleResult(result, (order) => {
    showSuccessNotification('Success! Order has been created with Order Number ' + order.orderNumber);
  });
});

function collectFormData(): OrderFormData {
  const skuElement = document.getElementById('sku') as HTMLInputElement;
  const quantityElement = document.getElementById('quantity') as HTMLInputElement;
  const countryElement = document.getElementById('country') as HTMLInputElement;

  const skuValue = skuElement?.value ?? '';
  const quantityValue = quantityElement?.value ?? '';
  const countryValue = countryElement?.value ?? '';

  return {
    sku: skuValue.trim(),
    quantity: parseInt(quantityValue),
    country: countryValue.trim(),
    quantityValue: quantityValue
  };
}

function validateFormData(data: OrderFormData): boolean {
  if (!data.sku) {
    showNotification('SKU is required', true);
    return false;
  }

  if (!data.quantityValue) {
    showNotification('Quantity is required', true);
    return false;
  }

  if (isNaN(data.quantity) || data.quantity <= 0) {
    showNotification('Quantity must be a positive number', true);
    return false;
  }

  if (!data.country) {
    showNotification('Country is required', true);
    return false;
  }

  if (data.country.length !== 2) {
    showNotification('Country must be a 2-letter code', true);
    return false;
  }

  return true;
}
