namespace Solster.AspNetCore.Components;

public class HtmlRendererException(
    String message,
    String? templateName = null,
    Type? componentType = null,
    Type? modelType = null,
    Exception? innerException = null)
    : Exception(message, innerException)
{
    public String? TemplateName { get; } = templateName;

    public Type? ComponentType { get; } = componentType;

    public Type? ModelType { get; } = modelType;
}

public sealed class TemplateResolverException(
    String templateName,
    String message,
    Exception? innerException = null)
    : HtmlRendererException(message, templateName: templateName, innerException: innerException);

public sealed class HtmlRendererComponentTypeException(
    String message,
    String? templateName = null,
    Type? componentType = null,
    Type? modelType = null,
    Exception? innerException = null)
    : HtmlRendererException(message, templateName, componentType, modelType, innerException);

public sealed class HtmlRendererModelBindingException(
    String message,
    String? templateName,
    Type componentType,
    Type modelType,
    Exception? innerException = null)
    : HtmlRendererException(message, templateName, componentType, modelType, innerException);

public sealed class HtmlRendererRenderException(
    String message,
    String? templateName,
    Type componentType,
    Type? modelType,
    Exception? innerException = null)
    : HtmlRendererException(message, templateName, componentType, modelType, innerException);

