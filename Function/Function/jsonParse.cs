using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Function.json
{
	public static class jsonParse
	{	

		public static TreeNode  String2TreeNode(string json)
		{
			TreeNode rtn = new TreeNode("jason");

			stringParse(rtn, json, 0);

			return rtn;
		}

		/// <summary>
		/// json 형식을 트리 뷰로 만들어 준다. name, text(value)
		/// </summary>
		/// <param name="tree"></param>
		/// <param name="json"></param>
		/// <param name="lv"></param>
		static void stringParse(TreeNode tree, string json, int lv)
		{
			string head;			
			string name;
			string value;
			int idx;

			if(json.StartsWith("}"))
			{
				lv--;

				// '}' 제거
				Fnc.StringGet(ref json, 1, 0);

				if (lv < 0)
				{
					if (json.StartsWith(",{"))
					{
						// ',' 제거
						Fnc.StringGet(ref json, 1, 0);

						lv = 0;

						stringParse(tree, json, lv);

						return;
					}
					else
						return;
				}

				

				stringParse(tree.Parent, json, lv);

				return;

			}
			else if (json.StartsWith("{") || json.StartsWith(","))
			{
				Fnc.StringGet(ref json, 1, 0);
			}

			//이름 부분
			if(json.StartsWith("\""))
			{
				Fnc.StringGet(ref json, 1, 0);
				idx = json.IndexOf("\"");
				name = Fnc.StringGet(ref json, idx);
				Fnc.StringGet(ref json, 1, 0);

				if (!json.StartsWith(":")) throw (new Exception("파싱 오류1"));

				// ':' 제거
				Fnc.StringGet(ref json, 1, 0);

				if (json.StartsWith("\""))
				{   //문자열 값

					Fnc.StringGet(ref json, 1, 0);
					idx = json.IndexOf("\"");
					value = Fnc.StringGet(ref json, idx);
					Fnc.StringGet(ref json, 1, 0);

					tree.Nodes.Add(name, value);

					//Console.WriteLine("[lv]{0} [Name]{1} [value]{2}", lv, name, value);

					stringParse(tree, json, lv);

				}
				else if(json.StartsWith("{"))
				{   //하부 배열

					TreeNode node = tree.Nodes.Add(name, "");

					//Console.WriteLine("[lv]{0} [Name]{1}", lv, name);

					lv++;
					stringParse(node, json, lv);

					return;
				}				
				else
				{
					//숫자열 값이거나 오류					
					idx = json.IndexOf(",");

					int idx2 = json.IndexOf("}");

					if (idx2 < idx) idx = idx2;


					if (idx < 0) throw (new Exception("파싱 오류3"));

					value = Fnc.StringGet(ref json, idx);

					//if(!Fnc.isNumeric(value.Replace("E","")) && value != "null") throw (new Exception("파싱 오류3"));
					
					tree.Nodes.Add(name, value);

					//Console.WriteLine("[lv]{0} [Name]{1} [value]{2}", lv, name, value);

					stringParse(tree, json, lv);
					
				}

			}
			else
			{
				throw (new Exception("파싱 오류"));
			}

			


		}





	}
}
