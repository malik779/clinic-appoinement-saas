namespace ClinicSaas.Api.Contracts;

public static class ApiResponseFactory
{
    public static ApiResponse<T> Success<T>(T data)
    {
        return new ApiResponse<T>(true, data, []);
    }

    public static ApiResponse<T> Failure<T>(params string[] errors)
    {
        return new ApiResponse<T>(false, default, errors);
    }

    public static ApiResponse<object> Failure(params string[] errors)
    {
        return new ApiResponse<object>(false, null, errors);
    }

    public static ApiResponse<object> Failure(IEnumerable<ClinicSaas.Application.Common.Error> errors)
    {
        return Failure(errors.Select(x => x.Message).ToArray());
    }
}
