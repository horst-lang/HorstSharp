using System;
using System.Collections.Generic;
using Horst.Parser;
using Horst.Tokens;

namespace Horst
{
    public class Environment
    {
        public Dictionary<string, dynamic> vars;
        public Dictionary<string, dynamic> privateVars;
        public Environment parent;

        public Environment(Environment parent)
        {
            this.parent = parent;
            vars = parent == null ? new Dictionary<string, dynamic>() : parent.vars;
            privateVars = new Dictionary<string, dynamic>();
        }

        public Environment Extend()
        {
            return new Environment(this);
        }

        public Environment Lookup(string name)
        {
            Environment scope = this;
            while (scope != null)
            {
                if (scope.privateVars.ContainsValue(name))
                {
                    return scope;
                }

                scope = scope.parent;
            }

            return null;
        }

        public dynamic Get(string name)
        {
            if (vars.ContainsValue(name))
            {
                return vars[name];
            }

            throw new Exception("Undefined variable " + name);
        }

        public Environment Set(string name, dynamic value)
        {
            Environment scope = Lookup(name);

            if (scope == null && parent != null)
            {
                throw new Exception("Undefined variable " + name);
            }

            return scope == null ? this : scope;
        }
        
    }
}