using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using System;
using System.IO;
using System.Xml;

namespace MetadataExtractCore.Extractors
{
    public abstract class XMPExtractor : DocumentExtractor
    {

        public XMPExtractor(Stream stm) : base(stm)
        { }

        protected void ExtractFromXMP(XmlDocument xDoc)
        {
            #region Metadatos como atributos
            XmlNodeList xnl = xDoc.GetElementsByTagName("rdf:Description");
            foreach (XmlNode xn in xnl)
            {
                XmlAttribute xa = xn.Attributes["pdf:Creator"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = Analysis.ApplicationAnalysis.GetApplicationsFromString(xa.Value);
                    if (!String.IsNullOrWhiteSpace(strValue))
                    {
                        this.foundMetadata.Add(new Application(strValue.Trim()));
                    }
                    //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                    else if (!String.IsNullOrWhiteSpace(xa.Value))
                    {
                        this.foundMetadata.Add(new Application(xa.Value.Trim()));
                    }
                }
                xa = xn.Attributes["pdf:CreationDate"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = xa.Value;
                    DateTime d;
                    if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                    {
                        //Si existe una fecha de creación anterior, sobreescribir
                        if (!foundMetadata.Dates.CreationDate.HasValue || this.foundMetadata.Dates.CreationDate > d)
                        {
                            this.foundMetadata.Dates.CreationDate = d;
                        }
                    }
                }
                xa = xn.Attributes["pdf:Title"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = xa.Value;
                    if (string.IsNullOrWhiteSpace(foundMetadata.Title) || this.foundMetadata.Title.Length < strValue.Length)
                        this.foundMetadata.Title = strValue;
                }
                xa = xn.Attributes["pdf:Author"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                    this.foundMetadata.Add(new User(xa.Value, true));
                xa = xn.Attributes["pdf:Producer"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = ApplicationAnalysis.GetApplicationsFromString(xa.Value);
                    if (!String.IsNullOrWhiteSpace(strValue))
                    {
                        this.foundMetadata.Add(new Application(strValue.Trim()));
                    }
                    //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                    else if (!String.IsNullOrWhiteSpace(xa.Value))
                    {
                        this.foundMetadata.Add(new Application(xa.Value.Trim()));
                    }
                }
                xa = xn.Attributes["pdf:ModDate"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = xa.Value;
                    DateTime d;
                    if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                    {
                        this.foundMetadata.Dates.ModificationDate = d;
                    }
                }
                xa = xn.Attributes["xap:CreateDate"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = xa.Value;
                    DateTime d;
                    if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                    {
                        //Si existe una fecha de creación anterior, sobreescribir
                        if (!foundMetadata.Dates.CreationDate.HasValue || this.foundMetadata.Dates.CreationDate > d)
                        {
                            //Si existe una fecha de modificación posterior, sobreescribir
                            if (!foundMetadata.Dates.ModificationDate.HasValue || this.foundMetadata.Dates.ModificationDate < d)
                            {
                                this.foundMetadata.Dates.CreationDate = d;
                            }
                        }
                    }
                }
                xa = xn.Attributes["xap:Title"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = xa.Value;
                    //Si ya existe un título y es mas pequeño, sobreescribirle.
                    if (String.IsNullOrWhiteSpace(foundMetadata.Title) || this.foundMetadata.Title.Length < strValue.Length)
                        this.foundMetadata.Title = strValue;
                }
                xa = xn.Attributes["xap:Author"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                    this.foundMetadata.Add(new User(xa.Value, true));
                xa = xn.Attributes["xap:ModifyDate"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = xa.Value;
                    DateTime d;
                    if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                    {
                        //Si existe una fecha de modificación posterior, sobreescribir
                        if (!foundMetadata.Dates.ModificationDate.HasValue || this.foundMetadata.Dates.ModificationDate < d)
                        {
                            this.foundMetadata.Dates.ModificationDate = d;
                        }
                    }
                }
                xa = xn.Attributes["xap:CreatorTool"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = Analysis.ApplicationAnalysis.GetApplicationsFromString(xa.Value);
                    if (!String.IsNullOrWhiteSpace(strValue))
                    {
                        this.foundMetadata.Add(new Application(strValue.Trim()));
                    }
                    //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                    else if (!String.IsNullOrWhiteSpace(xa.Value))
                    {
                        this.foundMetadata.Add(new Application(xa.Value.Trim()));
                    }
                }
                //xap:MetadataDate, fecha en la que se añadieron los metadatos
                xa = xn.Attributes["dc:title"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = xa.Value;
                    //Si ya existe un título y es mas pequeño, sobreescribirle.
                    if (string.IsNullOrEmpty(foundMetadata.Title) || this.foundMetadata.Title.Length < strValue.Length)
                        this.foundMetadata.Title = strValue;
                }
                xa = xn.Attributes["dc:creator"];
                if (xa != null && !string.IsNullOrEmpty(xa.Value))
                {
                    string strValue = xa.Value;
                    if (!string.IsNullOrEmpty(strValue))
                        this.foundMetadata.Add(new User(strValue, true));
                }
            }
            #endregion

            #region Metadatos como nodos independientes
            xnl = xDoc.GetElementsByTagName("pdf:Creator");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                string strValue = ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value);
                if (!String.IsNullOrWhiteSpace(strValue))
                {
                    this.foundMetadata.Add(new Application(strValue.Trim()));
                }
                //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                else if (!String.IsNullOrWhiteSpace(xnl[0].FirstChild.Value))
                {
                    this.foundMetadata.Add(new Application(xnl[0].FirstChild.Value.Trim()));
                }
            }
            xnl = xDoc.GetElementsByTagName("pdf:CreationDate");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                string strValue = xnl[0].FirstChild.Value;
                DateTime d;
                if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                {
                    //Si existe una fecha de creación anterior, sobreescribir
                    if (!foundMetadata.Dates.CreationDate.HasValue || this.foundMetadata.Dates.CreationDate > d)
                    {
                        this.foundMetadata.Dates.CreationDate = d;
                    }
                }
            }
            xnl = xDoc.GetElementsByTagName("pdf:Title");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                string strValue = xnl[0].FirstChild.Value;
                if ((string.IsNullOrEmpty(foundMetadata.Title) || this.foundMetadata.Title.Length < strValue.Length))
                    this.foundMetadata.Title = strValue;
            }
            xnl = xDoc.GetElementsByTagName("pdf:Author");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                this.foundMetadata.Add(new User(xnl[0].FirstChild.Value, true));
            xnl = xDoc.GetElementsByTagName("pdf:Producer");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                string strValue = ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value);
                if (!String.IsNullOrWhiteSpace(strValue))
                {
                    this.foundMetadata.Add(new Application(strValue.Trim()));
                }
                //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                else if (!String.IsNullOrWhiteSpace(xnl[0].FirstChild.Value))
                {
                    this.foundMetadata.Add(new Application(xnl[0].FirstChild.Value.Trim()));
                }
            }
            xnl = xDoc.GetElementsByTagName("pdf:ModDate");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                string strValue = xnl[0].FirstChild.Value;
                DateTime d;
                if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                {
                    this.foundMetadata.Dates.ModificationDate = d;
                }
            }
            xnl = xDoc.GetElementsByTagName("xap:CreateDate");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                string strValue = xnl[0].FirstChild.Value;
                DateTime d;
                if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                {
                    //Si existe una fecha de creación anterior, sobreescribir
                    if (!foundMetadata.Dates.CreationDate.HasValue || this.foundMetadata.Dates.CreationDate > d)
                    {
                        //Si existe una fecha de modificación posterior, sobreescribir
                        if (!foundMetadata.Dates.ModificationDate.HasValue || this.foundMetadata.Dates.ModificationDate < d)
                        {
                            this.foundMetadata.Dates.CreationDate = d;
                        }
                    }
                }
            }
            xnl = xDoc.GetElementsByTagName("xap:Title");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
            {
                XmlNode xn = xnl[0].FirstChild;
                //Busca el primer subnodo con valor
                while (xn.Value == null && xn.HasChildNodes)
                    xn = xn.FirstChild;
                if (!string.IsNullOrEmpty(xn.Value))
                {
                    string strValue = xn.Value;
                    //Si ya existe un título y es mas pequeño, sobreescribirle.
                    if (String.IsNullOrWhiteSpace(foundMetadata.Title) || this.foundMetadata.Title.Length < strValue.Length)
                        this.foundMetadata.Title = strValue;
                }
            }
            xnl = xDoc.GetElementsByTagName("xap:Author");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                this.foundMetadata.Add(new User(xnl[0].FirstChild.Value, true));
            }
            xnl = xDoc.GetElementsByTagName("xap:ModifyDate");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                string strValue = xnl[0].FirstChild.Value;
                DateTime d;
                if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                {
                    //Si existe una fecha de modificación posterior, sobreescribir
                    if (!foundMetadata.Dates.ModificationDate.HasValue || this.foundMetadata.Dates.ModificationDate < d)
                    {
                        this.foundMetadata.Dates.ModificationDate = d;
                    }
                }
            }
            xnl = xDoc.GetElementsByTagName("xap:CreatorTool");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                string strValue = Analysis.ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value);
                if (!String.IsNullOrWhiteSpace(strValue))
                {
                    this.foundMetadata.Add(new Application(strValue.Trim()));
                }
                //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                else if (!String.IsNullOrWhiteSpace(xnl[0].FirstChild.Value))
                {
                    this.foundMetadata.Add(new Application(Functions.ToPlainText(xnl[0].FirstChild.Value.Trim())));
                }
            }

            xnl = xDoc.GetElementsByTagName("dc:creator");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
            {
                XmlNode xn = xnl[0].FirstChild;
                //Busca el primer subnodo con valor
                while (xn.Value == null && xn.HasChildNodes)
                    xn = xn.FirstChild;
                if (!string.IsNullOrEmpty(xn.Value))
                {
                    string strValue = xn.Value;
                    this.foundMetadata.Add(new User(Functions.ToPlainText(strValue), true));
                }
            }

            //xap:MetadataDate, fecha en la que se añadieron los metadatos
            xnl = xDoc.GetElementsByTagName("dc:title");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
            {
                XmlNode xn = xnl[0].FirstChild;
                //Busca el primer subnodo con valor
                while (xn.Value == null && xn.HasChildNodes)
                    xn = xn.FirstChild;
                if (!string.IsNullOrEmpty(xn.Value))
                {
                    string strValue = xn.Value;
                    //Si ya existe un título y es mas pequeño, sobreescribirle.
                    if ((string.IsNullOrEmpty(foundMetadata.Title) || this.foundMetadata.Title.Length < strValue.Length))
                        this.foundMetadata.Title = strValue;
                }
            }

            xnl = xDoc.GetElementsByTagName("stRef:lastURL");
            if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
            {
                this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(xnl[0].FirstChild.Value), true));
            }

            #endregion
        }
    }
}
