using System;

namespace UnityEngine.Purchasing.Security
{
	public class AppleTangle
	{
		public static byte[] Data()
		{
			if (!AppleTangle.IsPopulated)
			{
				return null;
			}
			return Obfuscator.DeObfuscate(AppleTangle.data, AppleTangle.order, AppleTangle.key);
		}

		private static byte[] data = Convert.FromBase64String("br7VUvqpctj4PFWCpB3yKOr6qlaNIe8hUKqmpqKip5fFlqyXrqGk8tPPyNXO096WsZezoaTyo6S0qubXoZeooaTyurSmplijopekpqZYl7rdlyWm0ZepoaTyuqimplijo6SlprgiJCK8PprgkFUOPOcpi3MWN7V/zsHOxMbTzsjJh+bS08/I1c7T3pbAKK8Th1BsC4uHyNcRmKaXKxDkaIfk5pclpoWXqqGujSHvIVCqpqamqDqaVIzuj71vWWkSHql++btxbJqBl4OhpPKjrLS65tfXy8KH5MLV08nDh8TIycPO087IydSHyMGH0tTCh8jBh9PPwofTz8LJh8bX18vOxMbFy8KH1NPGycPG1cOH08LVytSHxpSR/ZfFlqyXrqGk8qOhtKXy9Ja0oEvaniQs9Id0n2MWGD3orcxYjFsMBNY14PTyZgiI5hRfXETXakEE66KnpCWmqKeXJaatpSWmpqdDNg6u7n/ROJSzwgbQM26KpaSmp6YEJab1wsvOxsnEwofIyYfTz87Uh8TC1SWmp6GujSHvIVDEw6KmlyZVl42hh8bJw4fEwtXTzsHOxMbTzsjJh9ehpPK6qaOxo7OMd87gM9GuWVPMKjI53asD4Cz8c7GQlGxjqOpps852ELwaNOWDtY1gqLoR6jv5xG/sJ7AZU9Q8SXXDqGze6JN/BZle31jMb9XGxNPOxMKH1NPG08LKwsnT1ImXuDZ8ueD3TKJK+d4jikyRBfDr8ktnxJTQUJ2gi/FMfaiGqX0d1L7oEtfLwofkwtXTzsHOxMbTzsjJh+bS0NCJxtfXy8KJxMjKiMbX18vCxMaSlZaTl5SR/bCqlJKXlZeelZaTlyezjHfO4DPRrllTzCqJ5wFQ4OrYi4fEwtXTzsHOxMbTwofXyMvOxN6vjKGmoqKgpaaxuc/T09fUnYiI0MOShLLssv66FDNQUTs5aPcdZv/3iecBUODq2K/5l7ihpPK6hKO/l7Gxl7OhpPKjpLSq5tfXy8KH9cjI08vCh+7JxImWgZeDoaTyo6y0uubXlyWjHJclpAQHpKWmpaWmpZeqoa5+kdhmIPJ+AD4eleVcf3LWOdkG9eLZuOvM9zHmLmPTxay3JOYglC0m087BzsTG08KHxd6Hxsneh9fG1dMPe9mFkm2Ccn6occxzBYOEtlAGC9jmDz9edm3BO4PMtncEHEO8jWS4Ep0KU6ippzWsFoaxidNym6p8xbEsvi55XuzLUqAMhZelT7+ZX/eudJqBwIctlM1QqiVoeUwEiF70zfzDkT7rit8QSis8e1TQPFXRddCX6GYWl/9L/aOVK88UKLp5wtRYwPnCG5e2oaTyo620rebX18vCh+7JxImWo6G0pfL0lrSXtqGk8qOttK3m19feh8bU1NLKwtSHxsTEwtfTxsnEwijUJsdhvPyuiDUVX+PvV8efObJSg0VMdhDXeKjiRoBtVsrfSkASsLD+AKKu27Dn8ba503QQLISc4ARyyIiXJmShr4yhpqKioKWllyYRvSYUqqGujSHvIVCqpqaioqekJaamp/vXy8KH9cjI04fk5pe5sKqXkZeTla/5lyWmtqGk8rqHoyWmr5clpqOX9w0tcn1DW3euoJAX0tKG");

		private static int[] order = new int[]
		{
			19, 1, 5, 29, 28, 49, 11, 21, 13, 22,
			10, 43, 41, 42, 42, 51, 24, 42, 27, 38,
			34, 45, 30, 27, 59, 50, 46, 51, 57, 57,
			36, 45, 32, 44, 35, 35, 45, 41, 57, 57,
			58, 50, 57, 54, 57, 51, 59, 56, 48, 59,
			51, 59, 58, 55, 55, 58, 59, 59, 58, 59,
			60
		};

		private static int key = 167;

		public static readonly bool IsPopulated = true;
	}
}
