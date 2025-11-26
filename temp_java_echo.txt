package com.optivem.eshop.systemtest.core.clients.system.api.controllers;

import com.optivem.eshop.systemtest.core.clients.commons.TestHttpClient;
import com.optivem.eshop.systemtest.core.clients.commons.TestHttpUtils;
import com.optivem.eshop.systemtest.core.commons.results.Result;

public class EchoController {

    private static final String ENDPOINT = "/echo";

    private final TestHttpClient httpClient;

    public EchoController(TestHttpClient httpClient) {
        this.httpClient = httpClient;
    }

    public Result<Void> echo() {
        var httpResponse = httpClient.get(ENDPOINT);
        return TestHttpUtils.getOkResultOrFailure(httpResponse);
    }
}

