using MD.CMS.BusinessLogic.Core.DataAccess.Controllers;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Options;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent;
using MD.CMS.BusinessLogic.Core.Helpers.Extensions;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.TypeConversion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.Core.Helpers.Calculations
{
    internal class PostfixEvaluator
    {
        /// <summary>
        /// Calculates postfix expression
        /// </summary>
        /// <param name="userMakingTheCall">The authenticated user making the call</param>
        /// <param name="fields">Reference to the fields in the content type</param>
        /// <param name="fieldValue">Reference to the field object in the content type</param>
        /// <param name="postfixExpression">Expression to be evaluated</param>
        /// <returns>result of the expression</returns>
        public static async Task EvaluateAsync(User userMakingTheCall, IEnumerable<GenericContentFieldValue> fields, GenericContentFieldValue fieldValue, string postfixExpression)
        {
            bool resultFound = false;
            string masterResult = string.Empty;
            try
            {
                if (userMakingTheCall is null)
                {
                    throw new ArgumentNullException(nameof(userMakingTheCall));
                }

                if (fields is null)
                {
                    throw new ArgumentNullException(nameof(fields));
                }

                if (string.IsNullOrEmpty(postfixExpression))
                {
                    throw new ArgumentException($"'{nameof(postfixExpression)}' cannot be null or empty", nameof(postfixExpression));
                }

                string regexField = @"\bfield\.[a-zA-Z1-9\[\]_]+";
                MatchCollection matches = Regex.Matches(postfixExpression, regexField);
                foreach (Match match in matches)
                {
                    string result = string.Empty;
                    string fieldName = match.Value.Replace("field.", string.Empty);
                    bool hasPropertyName = fieldName.Split('[').Length > 1;
                    string propertyName = string.Empty;
                    if (hasPropertyName)
                    {
                        propertyName = fieldName.Split('[')[1].Split(']')[0];
                        fieldName = fieldName.Split('[')[0];
                    }
                    GenericContentFieldValue fieldMatch = fields.FirstOrDefault(field => string.CompareOrdinal(field.FriendlyName, fieldName).Equals(0));
                    if (fieldMatch != null && fieldMatch.AttributeTypeDefinition != null)
                    {
                        switch (fieldMatch.AttributeTypeDefinition.InputType)
                        {
                            case DataAccess.Entities.AttributeTypeDefinition.EnumInputType.UserSelectorSingle:
                                {
                                    User user = await UserController.GetNewInstance().Caller(userMakingTheCall).GetByIdAsync(fieldMatch.Value);
                                    if (user != null)
                                    {
                                        if (hasPropertyName)
                                        {
                                            foreach (ProfileType profile in user.ProfileTypes)
                                            {
                                                string value = profile.GetFieldValue<string>(propertyName);
                                                if (!string.IsNullOrEmpty(value))
                                                {
                                                    result = value;
                                                    resultFound = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (!resultFound)
                                        {
                                            result = user.Username;
                                        }
                                    }
                                }
                                break;
                            case DataAccess.Entities.AttributeTypeDefinition.EnumInputType.TaxonomySelectorSingle:
                                {
                                    Taxonomy taxonomy = await TaxonomyController.GetNewInstance().Caller(userMakingTheCall).GetByIdAsync(fieldMatch.Value.ToInt64(default));
                                    if (taxonomy != null)
                                    {
                                        if (hasPropertyName)
                                        {
                                            switch (propertyName)
                                            {
                                                case "Name":
                                                    result = taxonomy.Name;
                                                    resultFound = true;
                                                    break;
                                                case "TaxonomyPath":
                                                    result = taxonomy.TaxonomyPath;
                                                    resultFound = true;
                                                    break;
                                            }
                                        }
                                        if (!resultFound)
                                        {
                                            result = taxonomy.Name;
                                        }
                                    }
                                }
                                break;
                            case DataAccess.Entities.AttributeTypeDefinition.EnumInputType.ContentSelectorSingle:
                                {
                                    string id = string.Empty;
                                    int lcid = Properties.Settings.Default.DefaultLcid;
                                    if (fieldMatch.Value.Split(fieldMatch.Delimiter).Count() > 1)
                                    {
                                        id = fieldMatch.Value.Split(fieldMatch.Delimiter).First();
                                    }
                                    if (fieldMatch.Value.Split('-').Count() > 1)
                                    {
                                        id = fieldMatch.Value.Split('-').First();
                                        lcid = fieldMatch.Value.Split('-').Last().ToInt32(lcid);
                                    }
                                    Content content = (await ContentController<Content>.GetNewInstance().Caller(userMakingTheCall).GetByIdAsync(new ContentOptions
                                    {
                                        ContentIds = new string[] { id }.ToList(),
                                        Lcid = lcid,
                                        FillFields = true
                                    })).FirstOrDefault();
                                    if (content != null)
                                    {
                                        if (hasPropertyName && content.ContentType != null)
                                        {
                                            string value = await content.ContentType.GetFieldValueAsync<string>(propertyName);
                                            if (!string.IsNullOrEmpty(value))
                                            {
                                                result = value;
                                                resultFound = true;
                                            }
                                        }
                                        if (hasPropertyName && !resultFound)
                                        {
                                            switch (propertyName)
                                            {
                                                case "Title":
                                                    result = content.Title;
                                                    resultFound = true;
                                                    break;
                                                case "Path":
                                                    result = content.Path;
                                                    resultFound = true;
                                                    break;
                                                case "UniqueId":
                                                    result = content.UniqueId;
                                                    resultFound = true;
                                                    break;
                                            }
                                        }
                                        if (!resultFound)
                                        {
                                            result = content.Title;
                                        }
                                    }
                                }
                                break;
                            case DataAccess.Entities.AttributeTypeDefinition.EnumInputType.MediaContentSelectorSingle:
                                {
                                    MediaContent mediaContent = await MediaContentController.GetNewInstance().Caller(userMakingTheCall).GetByIdAsync(fieldMatch.Value.ToInt64(default));
                                    if (mediaContent != null)
                                    {
                                        switch (propertyName)
                                        {
                                            case "Name":
                                                result = mediaContent.Name;
                                                resultFound = true;
                                                break;
                                            case "Description":
                                                result = mediaContent.Description;
                                                resultFound = true;
                                                break;
                                            case "Path":
                                                result = mediaContent.Path;
                                                resultFound = true;
                                                break;
                                            case "UniqueId":
                                                result = mediaContent.UniqueId;
                                                resultFound = true;
                                                break;
                                        }
                                        if (!resultFound)
                                        {
                                            result = mediaContent.Name;
                                        }
                                    }
                                }
                                break;
                            default:
                                result = fieldMatch.Value;
                                break;
                        }
                    }
                    postfixExpression = postfixExpression.Replace(match.Value, ParseValue(result));
                }

                postfixExpression = postfixExpression.Replace("'", "\"");

                postfixExpression = postfixExpression.Replace("Math.round(", "Math.Round((double)", StringComparison.OrdinalIgnoreCase);

                masterResult = EvaluateSafeExpression(postfixExpression);
            }
            catch (ArgumentNullException e)
            {
                typeof(PostfixEvaluator).Log(e);
            }
            catch (ArgumentException e)
            {
                typeof(PostfixEvaluator).Log(e);
            }
            catch (Exception e)
            {
                typeof(PostfixEvaluator).Log(e);
            }
            fieldValue.Value = masterResult;
        }

        private static string ParseValue(string input)
        {
            if(!int.TryParse(input, out _))
            {
                input = $"\"{input}\"";
            }
            return input;
        }

        private static string EvaluateSafeExpression(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return string.Empty;
            }

            string candidate = expression.Trim();
            candidate = candidate.Replace("Math.round(", "Math.Round((double)", StringComparison.OrdinalIgnoreCase);

            // Support one known function form while blocking arbitrary code execution.
            candidate = Regex.Replace(candidate, @"Math\.Round\(\(double\)\s*([-+*/().\d\s]+)\)", match =>
            {
                var innerValue = Convert.ToDouble(new DataTable().Compute(match.Groups[1].Value, string.Empty));
                return Math.Round(innerValue).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }, RegexOptions.IgnoreCase);

            if (Regex.IsMatch(candidate, @"[A-Za-z_]"))
            {
                throw new ArgumentException("Expression contains unsupported tokens.", nameof(expression));
            }

            object result = new DataTable().Compute(candidate, string.Empty);
            return result?.ToString() ?? string.Empty;
        }
    }
}