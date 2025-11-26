package com.optivem.eshop.systemtest.core.clients.commons;

import com.microsoft.playwright.Locator;
import com.microsoft.playwright.Page;
import com.microsoft.playwright.options.WaitForSelectorState;

import java.math.BigDecimal;

public class TestPageClient {
    private final Page page;
    private final String baseUrl;
    private final int timeoutMilliseconds;

    private static final int DEFAULT_TIMEOUT_SECONDS = 10;
    private static final int DEFAULT_TIMEOUT_MILLISECONDS = DEFAULT_TIMEOUT_SECONDS * 1000;

    private TestPageClient(Page page, String baseUrl, int timeoutMilliseconds) {
        this.page = page;
        this.baseUrl = baseUrl;
        this.timeoutMilliseconds = timeoutMilliseconds;
    }

    public TestPageClient(Page page, String baseUrl) {
        this(page, baseUrl, DEFAULT_TIMEOUT_MILLISECONDS);
    }

    public String getBaseUrl() {
        return baseUrl;
    }

    public Page getPage() {
        return page;
    }

    public void fill(String selector, String text) {
        var input = page.locator(selector);
        wait(input);
        input.fill(text);
    }

    public void click(String selector) {
        var button = page.locator(selector);
        wait(button);
        button.click();
    }

    public String readTextContent(String selector) {
        var locator = page.locator(selector);
        wait(locator);
        return locator.textContent();
    }

    public boolean exists(String selector) {
        var locator = page.locator(selector);
        try {
            wait(locator);
            return locator.count() > 0;
        } catch (Exception e) {
            return false;
        }
    }

    public String readInputValue(String selector) {
        var locator = page.locator(selector);
        wait(locator);
        return locator.inputValue();
    }

    public BigDecimal readInputCurrencyDecimalValue(String selector) {
        var inputValue = readInputValue(selector);
        inputValue = inputValue.replace("$", "");
        return new BigDecimal(inputValue);
    }

    public BigDecimal readInputPercentageDecimalValue(String selector) {
        var inputValue = readInputValue(selector);
        inputValue = inputValue.replace("%", "");
        return new BigDecimal(inputValue);
    }

    public boolean isHidden(String selector) {
        var locator = page.locator(selector);
        return locator.count() == 0;
    }

    public void waitForHidden(String selector) {
        var waitForOptions = getWaitForOptions()
                .setState(WaitForSelectorState.HIDDEN)
                .setTimeout(timeoutMilliseconds);

        var locator = page.locator(selector);
        locator.waitFor(waitForOptions);
    }

    public void waitForVisible(String selector) {
        var waitForOptions = getWaitForOptions()
                .setState(WaitForSelectorState.VISIBLE)
                .setTimeout(timeoutMilliseconds);

        var locator = page.locator(selector);
        locator.waitFor(waitForOptions);
    }

    private void wait(Locator locator) {
        var waitForOptions = getWaitForOptions();
        locator.waitFor(waitForOptions);
    }

    private Locator.WaitForOptions getWaitForOptions() {
        return new Locator.WaitForOptions().setTimeout(timeoutMilliseconds);
    }
}

