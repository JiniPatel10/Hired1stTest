namespace backendApi.Middleware
{
    public enum ErrorCodes
    {
        ValidationModelError = 1000,
        CreatingUserError = 1001,
        GettlingAllUsersError = 1002,
        GettingUserByIdError = 1003,
        UpdatingUserError = 1004,
        DeletingUserError = 1005,
        CreatingProductError = 1006,
        GettlingAllProductError = 1007,
        GettingProductByIdError = 1008,
        UpdatingProductError = 1009,
        DeletingProductError = 1010,
        ValidatingUserError = 1011,
        SendMailError = 1012
    }
}
