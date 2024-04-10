﻿namespace DocumentCrud.WebAPI.Extentions;

public static class EndpointRouteBuilderExtention
{
    public static RouteHandlerBuilder ProducesGet<T>(this RouteHandlerBuilder builder) => builder
        .Produces<T>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

    public static RouteHandlerBuilder ProducesPost(this RouteHandlerBuilder builder) => builder
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError);

    public static RouteHandlerBuilder ProducesPut(this RouteHandlerBuilder builder) => builder
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError);

    public static RouteHandlerBuilder ProducesDelete(this RouteHandlerBuilder builder) => builder
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
}

