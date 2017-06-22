using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Function.Util
{
	/// <summary>
	/// xml 관리 클래스
	/// </summary>
    public class XML : IDisposable
    {
        public enum enXmlType { File, String };

        public XmlDocument xml;
		public XmlNode xmlNode;


        public XML(enXmlType enxmlType, string strXml)
        {
            try
            {
                xml = new XmlDocument();
                

                
                switch (enxmlType)
                {
                    //xml이 파일
                    case enXmlType.File:
                        xml.Load(strXml);
                        break;
                    
                    //xml이 문자열.
                    case enXmlType.String:
                        xml.LoadXml(strXml);
                        break;
                }

                
            }
            catch(Exception ex)
            {
                xml = null;
                throw ex;
            }
        }

		public XML() { }

        /// <summary>
        /// 전체 Node경로 값을 검색한다.
        /// </summary>
        /// <param name="strNode"></param>
        /// <returns></returns>
        public string ReadNodeValue(string strNode)
        {
            try
            {
                int i = 0;
                string strRst = string.Empty;

                //현재 Node를 초기화
                this.chNode2Root();
                                
                strNode = strNode.Replace(@".//", string.Empty);

                string[] strPath = strNode.Split(char.Parse(@"/"));

                if (strPath.Length < 1) return string.Empty;

                

                foreach (string str in strPath)
                {
                    i ++;
                    if (i == strPath.Length)
                        strRst = GetSingleNodeValue(str);
                    else
                        chSingleNode(str);

                }


                return strRst;                
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 싱글 노드 경로를 변경한다.
        /// </summary>
        /// <param name="strNode"></param>
        /// <returns></returns>
        public bool chSingleNode(string strNode)
        {
            try
            {
                if (xmlNode == null)
                    xmlNode = xml.DocumentElement.SelectSingleNode(strNode);
                else
                    xmlNode = xmlNode.SelectSingleNode(strNode);


				if (xmlNode == null) throw new Exception(string.Format("[{0}] 해당 Node가 존재 하지 않습니다.", strNode));
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


		/// <summary>
		/// NodeList를 조회한다.
		/// </summary>
		/// <param name="strNode"></param>
		/// <returns></returns>
		public XmlNodeList GetNodeList(string strNode)
		{
			try
			{
				if (xmlNode == null)
					return xml.DocumentElement.SelectNodes(strNode);
				else
					return xmlNode.SelectNodes(strNode);
				
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetCurrentNodePath(string strNode)
		{
			if (xmlNode == null)
				return strNode == string.Empty ? string.Empty : @".//" + strNode;

			return "@.//" + xmlNode.Name + (strNode == string.Empty?string.Empty:@"//" + strNode);
			
		}

        /// <summary>
        /// Node에 값을 구한다.
        /// </summary>
        /// <param name="strNode"></param>
        /// <returns></returns>
        public string GetSingleNodeValue(string strNode)
        {
            try
            {
                string strValue = string.Empty;

                if (xmlNode == null)
                    strValue = xml.SelectSingleNode(@".//" + strNode).InnerText.Trim();
                else
                    strValue = xmlNode.SelectSingleNode(strNode).InnerText.Trim();

                return strValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


		/// <summary>
		/// 하부 노드의 값을 구한다.
		/// </summary>
		/// <param name="strNode"></param>
		/// <returns></returns>
		public string [] GetChildsNodeValues()
		{
			try
			{			

				XmlNode xn;
				if (xmlNode == null)
					xn = (XmlNode)xml;
				else
					xn = xmlNode;


				string[] strValue = new string[xn.ChildNodes.Count];

				int i = 0;
				foreach (XmlNode n in xn.ChildNodes)
				{
					strValue[i] = n.InnerText.Trim();
					i++;
				}
				
				return strValue;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// 특정 Lode에 값을 구한다.
		/// </summary>
		/// <param name="strNode"></param>
		/// <returns></returns>
		public string[] GetSelectNodeValues(string strCondition)
		{
			try
			{

				XmlNode xn;
				if (xmlNode == null)
					xn = (XmlNode)xml;
				else
					xn = xmlNode;

				XmlNodeList nl = xn.SelectNodes(strCondition);

				string[] strValue = new string[nl.Count];

				int i = 0;
				foreach (XmlNode n in nl)
				{
					strValue[i] = n.InnerText.Trim();
					i++;
				}

				return strValue;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// Node에 값을 저장한다.
		/// </summary>
		/// <param name="strNode"></param>
		/// <returns></returns>
		public void SetSingleNodeValue(string strNode, string strValue)
		{
			try
			{
				if (xmlNode == null)
					xml.DocumentElement.SelectSingleNode(@".//" + strNode).InnerText = strValue;
				else
				{
					//string strnode = @".//" + xmlNode.Name;

					//XmlNode node = xml.DocumentElement.SelectSingleNode(strNode);

					//strValue = xmlNode.SelectSingleNode(strNode).InnerText.Trim();

					//if (strNode != string.Empty)
					//    strnode += "/" + strNode;

					//xml.SelectSingleNode(strnode).Value = strValue;

					xmlNode.SelectSingleNode(strNode).InnerText = strValue;

					//node.InnerText = strValue;
						
					
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Child Node를 추가한다.
		/// </summary>
		/// <param name="strNode"></param>
		/// <returns></returns>
		public void AddChild(string strNode, string strChileNodeName, string strValue)
		{
			try
			{
				
								
				XmlElement child = xml.CreateElement(strChileNodeName);
				child.InnerText = strValue;

				XmlNode xn;

				if (xmlNode == null)
					xn = xml.SelectSingleNode(strNode);
				else
					xn = xmlNode.SelectSingleNode(strNode);

				xn.AppendChild(child);

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Child Node를 추가한다.
		/// </summary>
		/// <param name="strNode"></param>
		/// <returns></returns>
		public void AddChild(string strChileNodeName, string strValue)
		{
			try
			{
								
				XmlElement child = xml.CreateElement(strChileNodeName);
				child.InnerText = strValue;
				
				if (xmlNode == null)					
					xml.DocumentElement.AppendChild(child);
				else
					xmlNode.AppendChild(child);

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// Attribute가 있는 Child Node를 추가한다.
		/// </summary>
		/// <param name="strNode"></param>
		/// <returns></returns>
		public void AddAttChild(string strNode, string strChildNodeName, string strValue, string strAttributeName, string strAttributeValue)
		{
			try
			{				

				XmlElement child = xml.CreateElement(strChildNodeName);
				child.InnerText = strValue;

				XmlAttribute att = xml.CreateAttribute(strAttributeName);
				att.Value = strAttributeValue;

				child.Attributes.Append(att);				

				XmlNode xn;

				if (xmlNode == null)
					xn = xml.SelectSingleNode(strNode);
				else
					xn = xmlNode.SelectSingleNode(strNode);

				xn.AppendChild(child);				

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		/// <summary>
		/// InnerXml을 구한다.
		/// </summary>
		/// <param name="strNode"></param>
		/// <returns></returns>
		public string GetSingleNodeInnerXml(string strNode)
		{
			try
			{
				string strValue = string.Empty;

				//if (xmlNode == null)
				//    strValue = xml.SelectSingleNode(@".//" + strNode).InnerXml;
				//else
				//    strValue = xmlNode.SelectSingleNode(strNode).InnerXml;

				string strnode;

				if (xmlNode == null)
					strnode = string.Empty;
				else
					strnode = xmlNode.Name;

				if (strNode != string.Empty)
				{
					if (strnode == string.Empty) strnode = @".//";
						
					strnode += strNode;
				}

				if (strnode == string.Empty)
					return xml.InnerXml;
				else
					return xml.SelectSingleNode(strnode).InnerXml;



			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public void SaveToFile(string strFilePath)
		{
			xml.Save(strFilePath);
		}


		/// <summary>
		/// 현재 xmlNode를 반환한다.
		/// </summary>
		public XmlNode GetXmlNode
		{
			get
			{
				if (xmlNode == null)
					return (XmlNode)xml;
				else
					return xmlNode;

			}
		}

        /// <summary>
        /// 현재 선택 Node를 루트로 변경..
        /// </summary>
        public void chNode2Root()
        {
            xmlNode = null;
        }


        public void Dispose() 
        {
            xmlNode = null;
            xml = null;

			
        }




		public static class Fnc
		{
			/// <summary>
			/// 현재 노드의 전체 경로를 반환한다.
			/// </summary>
			/// <param name="xn"></param>
			/// <returns></returns>
			public static string NodePath_Get(XmlNode xn)
			{
				if (xn.ParentNode != null && xn.ParentNode.Name != @"#document")
					return string.Format(@"{0}\{1}", NodePath_Get(xn.ParentNode), xn.Name);
				else
					return string.Format(@"\{0}",xn.Name);

			}
		}






		public enum EncodingType { UTF_8, euc_kr };

		private string EncodingType2String(EncodingType eType)
		{
			return eType.ToString().Replace("_", "-");
		}

		/// <summary>
		/// xml 문서를 만든다.
		/// </summary>
		/// <param name="eType"></param>
		/// <param name="isStandAlone"></param>
		public void XML_Create(EncodingType eType, bool isStandAlone, stNodeValue RootNode)
		{
			xml = new XmlDocument();

			XmlDeclaration dec = xml.CreateXmlDeclaration("1.0", EncodingType2String(eType), isStandAlone ? "yes" : "no");

			xml.InsertBefore(dec, xml.DocumentElement);

			if (RootNode != null) Node_Add(RootNode, null);


			chNode2Root();

			//chSingleNode(RootNode.Name);


		}

		public class stNodeValue
		{
			/// <summary>
			/// 새 노드의 지역 이름입니다.
			/// </summary>
			public string Name = string.Empty;
			/// <summary>
			/// 새 노드의 접두사입니다.
			/// (생략가능)
			/// </summary>
			public string preFix = string.Empty;
			/// <summary>
			/// 새 노드의 네임스페이스 URI입니다.
			/// </summary>
			public string namespaceUri = string.Empty;
			/// <summary>
			/// Attribute 설정시 값
			/// </summary>
			public string Value = string.Empty;
			/// <summary>
			/// Node설정시 내부 텍스트
			/// </summary>
			public string InnerText = string.Empty;

			public stNodeValue()
			{
			}

			/// <summary>
			/// 속성용 값을 초기화 합니다.
			/// </summary>
			/// <param name="_Name"></param>
			/// <param name="_Value">string 값입력</param>
			public stNodeValue(string _Name, object _Value)
			{
				Name = _Name;
				Value = _Value.ToString();
			}

			/// <summary>
			/// node용 값을 초기화 합니다.
			/// </summary>
			/// <param name="_Name"></param>
			/// <param name="_InnerText"></param>
			public stNodeValue(string _Name, string _InnerText)
			{
				Name = _Name;
				InnerText = _InnerText;
			}

		}


		/// <summary>
		/// xml에 노드를 추가 한다.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="Atts">추가할 속성 없으면 null</param>
		public void Node_Add(stNodeValue node, stNodeValue[] Atts)
		{
			if (node.namespaceUri == null) node.namespaceUri = string.Empty;

			if (node.InnerText == null) node.InnerText = string.Empty;

			XmlNode xn;


			if (node.preFix == null || node.preFix == string.Empty)
				xn = xml.CreateNode(XmlNodeType.Element, node.Name, node.namespaceUri);
			else
				xn = xml.CreateNode(XmlNodeType.Element, node.preFix, node.Name, node.namespaceUri);


			if (Atts != null)
				Node_Attribute_Add(xn, Atts);

			xn.InnerText = node.InnerText;

			if (xmlNode == null)
				xml.AppendChild(xn);
			else
				xmlNode.AppendChild(xn);

		}


		/// <summary>
		/// node에 속성을 추가 하여 준다.
		/// </summary>
		/// <param name="xn"></param>
		/// <param name="Atts"></param>
		public void Node_Attribute_Add(XmlNode xn, stNodeValue[] Atts)
		{
			foreach (stNodeValue Att in Atts)
			{
				XmlNode att;

				if (Att.preFix == null || Att.preFix == string.Empty)
					att = xml.CreateNode(XmlNodeType.Attribute, Att.Name, Att.namespaceUri);
				else
					att = xml.CreateNode(XmlNodeType.Attribute, Att.preFix, Att.Name, Att.namespaceUri);

				att.Value = Att.Value;

				xn.Attributes.SetNamedItem(att);

			}
		}

		public void Node_Find()
		{
			
		}



    }
}
