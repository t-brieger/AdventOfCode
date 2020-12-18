using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day12 : Solution
    {
        public override string Part1(string input)
        {
            long getSum(JToken jo)
            {
                long sum = 0;
                foreach (JToken element in jo)
                {
                    sum += element.Type switch
                    {
                        JTokenType.Integer => ((JValue) element).Value as long? ?? 0,
                        JTokenType.Object or JTokenType.Array or JTokenType.Property => getSum(element),
                        JTokenType.String => 0,
                        _ => throw new Exception(element.Type.ToString())
                    };
                }

                return sum;
            }

            return getSum(JToken.Parse(input)).ToString();
        }

        public override string Part2(string input)
        {
            long getSum(JToken jo)
            {
                long sum = 0;
                //wtf is this
                if (jo.Any(jt =>
                    jt.Type == JTokenType.Property && ((JProperty) jt).Value is JValue &&
                    ((JValue) ((JProperty) jt).Value).Value as string == "red"))
                    return 0;
                foreach (JToken element in jo)
                {
                    switch (element.Type)
                    {
                        case JTokenType.Integer:
                            sum += ((JValue) element).Value as long? ?? 0;
                            break;
                        case JTokenType.Property:
                        case JTokenType.Object:
                        case JTokenType.Array:
                            long x = getSum(element);
                            sum += x;
                            break;
                        case JTokenType.String:
                            break;
                        default:
                            throw new Exception(element.Type.ToString());
                    }
                }

                return sum;
            }

            return getSum(JToken.Parse(input)).ToString();
        }
    }
}
