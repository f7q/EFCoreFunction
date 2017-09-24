using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class MyNpgsqlCompositeMethodCallTranslator : NpgsqlCompositeMethodCallTranslator
    {
        static readonly IMethodCallTranslator[] _methodCallTranslators =
        {
            new NpgsqlFTSTranslator(),               //add
            new NpgsqlArraySequenceEqualTranslator(),
            new NpgsqlConvertTranslator(),
            new NpgsqlStringSubstringTranslator(),
            new NpgsqlLikeTranslator(),
            new NpgsqlMathTranslator(),
            new NpgsqlObjectToStringTranslator(),
            new NpgsqlStringEndsWithTranslator(),
            new NpgsqlStringStartsWithTranslator(),
            new NpgsqlStringContainsTranslator(),
            new NpgsqlStringIndexOfTranslator(),
            new NpgsqlStringIsNullOrWhiteSpaceTranslator(),
            new NpgsqlStringReplaceTranslator(),
            new NpgsqlStringToLowerTranslator(),
            new NpgsqlStringToUpperTranslator(),
            new NpgsqlStringTrimTranslator(),
            new NpgsqlStringTrimEndTranslator(),
            new NpgsqlStringTrimStartTranslator(),
            new NpgsqlRegexIsMatchTranslator()
        };

        public MyNpgsqlCompositeMethodCallTranslator(
             [NotNull] RelationalCompositeMethodCallTranslatorDependencies dependencies)
            : base(dependencies)
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            AddTranslators(_methodCallTranslators);
        }
    }
}