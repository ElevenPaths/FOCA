namespace FOCA.GUI.Navigation
{
    public static class Project
    {
        public static string Key
        {
            get
            {
                return "Project";
            }
        }
    
        public static string ToNavigationPath()
        {
            return "/" + Key;
        }
    
        public static class Network
        {
            public static string Key
            {
                get
                {
                    return "Network";
                }
            }
        
            public static string ToNavigationPath()
            {
                return Project.ToNavigationPath() + "/" + Key;
            }
        
            public static class Servers
            {
                public static string Key
                {
                    get
                    {
                        return "Servers";
                    }
                }
            
                public static string ToNavigationPath()
                {
                    return Network.ToNavigationPath() + "/" + Key;
                }
            
                public static class Unknown
                {
                    public static string Key
                    {
                        get
                        {
                            return "Unknown";
                        }
                    }
                
                    public static string ToNavigationPath()
                    {
                        return Servers.ToNavigationPath() + "/" + Key;
                    }
                
                }
            }
            public static class Clients
            {
                public static string Key
                {
                    get
                    {
                        return "Clients";
                    }
                }
            
                public static string ToNavigationPath()
                {
                    return Network.ToNavigationPath() + "/" + Key;
                }
            
            }
        }
        public static class Domains
        {
            public static string Key
            {
                get
                {
                    return "Domains";
                }
            }
        
            public static string ToNavigationPath()
            {
                return Project.ToNavigationPath() + "/" + Key;
            }
        
        }
        public static class DocumentAnalysis
        {
            public static string Key
            {
                get
                {
                    return "DocumentAnalysis";
                }
            }
        
            public static string ToNavigationPath()
            {
                return Project.ToNavigationPath() + "/" + Key;
            }
        
            public static class Files
            {
                public static string Key
                {
                    get
                    {
                        return "Files";
                    }
                }
            
                public static string ToNavigationPath()
                {
                    return DocumentAnalysis.ToNavigationPath() + "/" + Key;
                }
            
            }
            public static class MetadataSummary
            {
                public static string Key
                {
                    get
                    {
                        return "MetadataSummary";
                    }
                }
            
                public static string ToNavigationPath()
                {
                    return DocumentAnalysis.ToNavigationPath() + "/" + Key;
                }
            
                public static class Users
                {
                    public static string Key
                    {
                        get
                        {
                            return "Users";
                        }
                    }
                
                    public static string ToNavigationPath()
                    {
                        return MetadataSummary.ToNavigationPath() + "/" + Key;
                    }
                
                }
                public static class Folders
                {
                    public static string Key
                    {
                        get
                        {
                            return "Folders";
                        }
                    }
                
                    public static string ToNavigationPath()
                    {
                        return MetadataSummary.ToNavigationPath() + "/" + Key;
                    }
                
                }
                public static class Printers
                {
                    public static string Key
                    {
                        get
                        {
                            return "Printers";
                        }
                    }
                
                    public static string ToNavigationPath()
                    {
                        return MetadataSummary.ToNavigationPath() + "/" + Key;
                    }
                
                }
                public static class Software
                {
                    public static string Key
                    {
                        get
                        {
                            return "Software";
                        }
                    }
                
                    public static string ToNavigationPath()
                    {
                        return MetadataSummary.ToNavigationPath() + "/" + Key;
                    }
                
                }
                public static class Emails
                {
                    public static string Key
                    {
                        get
                        {
                            return "Emails";
                        }
                    }
                
                    public static string ToNavigationPath()
                    {
                        return MetadataSummary.ToNavigationPath() + "/" + Key;
                    }
                
                }
                public static class OperatingSystems
                {
                    public static string Key
                    {
                        get
                        {
                            return "OperatingSystems";
                        }
                    }
                
                    public static string ToNavigationPath()
                    {
                        return MetadataSummary.ToNavigationPath() + "/" + Key;
                    }
                
                }
                public static class Passwords
                {
                    public static string Key
                    {
                        get
                        {
                            return "Passwords";
                        }
                    }
                
                    public static string ToNavigationPath()
                    {
                        return MetadataSummary.ToNavigationPath() + "/" + Key;
                    }
                
                }
                public static class Servers
                {
                    public static string Key
                    {
                        get
                        {
                            return "Servers";
                        }
                    }
                
                    public static string ToNavigationPath()
                    {
                        return MetadataSummary.ToNavigationPath() + "/" + Key;
                    }
                
                }
            }
            public static class MalwareSummary
            {
                public static string Key
                {
                    get
                    {
                        return "MalwareSummary";
                    }
                }
            
                public static string ToNavigationPath()
                {
                    return DocumentAnalysis.ToNavigationPath() + "/" + Key;
                }
            
            }
        }
    }
}