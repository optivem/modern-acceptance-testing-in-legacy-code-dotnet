package com.optivem.eshop.systemtest.core.clients.commons;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.optivem.eshop.systemtest.core.commons.dtos.ProblemDetailResponse;
import com.optivem.eshop.systemtest.core.commons.results.Result;
import org.springframework.http.HttpStatus;

import java.net.URI;
import java.net.http.HttpResponse;
import java.util.ArrayList;
import java.util.List;

public class TestHttpUtils {
    private static final ObjectMapper objectMapper = new ObjectMapper();

    public static <T> T readResponse(HttpResponse<String> httpResponse, Class<T> responseType) {
        try {
            var responseBody = httpResponse.body();
            return objectMapper.readValue(responseBody, responseType);
        } catch (Exception ex) {
            throw new RuntimeException("Failed to deserialize response to " + responseType.getSimpleName(), ex);
        }
    }

    public static String serializeRequest(Object request) {
        try {
            return objectMapper.writeValueAsString(request);
        } catch (Exception ex) {
            throw new RuntimeException("Failed to serialize request object", ex);
        }
    }

    public static List<String> getErrorMessages(HttpResponse<String> httpResponse) {
        var problemDetail = readResponse(httpResponse, ProblemDetailResponse.class);

        var errors = new ArrayList<String>();

        if (problemDetail.getDetail() != null) {
            errors.add(problemDetail.getDetail());
        }

        if(problemDetail.getErrors() != null) {
            for (var error : problemDetail.getErrors()) {
                errors.add(error.getMessage());
            }
        }

        return errors;
    }



    public static <T> Result<T> getOkResultOrFailure(HttpResponse<String> httpResponse, Class<T> responseType) {
        return getResultOrFailure(httpResponse, responseType, HttpStatus.OK);
    }

    public static Result<Void> getOkResultOrFailure(HttpResponse<String> httpResponse) {
        return getResultOrFailure(httpResponse, HttpStatus.OK);
    }

    public static <T> Result<T> getCreatedResultOrFailure(HttpResponse<String> httpResponse, Class<T> responseType) {
        return getResultOrFailure(httpResponse, responseType, HttpStatus.CREATED);
    }

    public static Result<Void> getCreatedResultOrFailure(HttpResponse<String> httpResponse) {
        return getResultOrFailure(httpResponse, HttpStatus.CREATED);
    }

    public static Result<Void> getNoContentResultOrFailure(HttpResponse<String> httpResponse) {
        var isSuccess = hasStatusCode(httpResponse, HttpStatus.NO_CONTENT);

        if (!isSuccess) {
            var errorMessages = getErrorMessages(httpResponse);
            return Result.failure(errorMessages);
        }

        return Result.success();
    }

    public static URI getUri(String baseUrl, String path) {
        try {
            return new URI(baseUrl + path);
        } catch (Exception ex) {
            throw new RuntimeException("Failed to create URI for path: " + path, ex);
        }
    }

    private static boolean hasStatusCode(HttpResponse<String> httpResponse, HttpStatus statusCode) {
        return httpResponse.statusCode() == statusCode.value();
    }

    private static <T> Result<T> getResultOrFailure(HttpResponse<String> httpResponse, Class<T> responseType, HttpStatus successStatus) {
        var isSuccess = TestHttpUtils.hasStatusCode(httpResponse, successStatus);

        if(!isSuccess) {
            var errorMessages = getErrorMessages(httpResponse);
            return Result.failure(errorMessages);
        }

        var response = TestHttpUtils.readResponse(httpResponse, responseType);
        return Result.success(response);
    }

    private static Result<Void> getResultOrFailure(HttpResponse<String> httpResponse, HttpStatus successStatus) {
        var isSuccess = TestHttpUtils.hasStatusCode(httpResponse, successStatus);

        if(!isSuccess) {
            var errorMessages = getErrorMessages(httpResponse);
            return Result.failure(errorMessages);
        }

        return Result.success();
    }

}
