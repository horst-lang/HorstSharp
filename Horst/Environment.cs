using System;
using System.Collections.Generic;

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
            if (vars.ContainsKey(name))
            {
                return vars[name];
            }

            throw new Exception($"Undefined variable '{name}'");
        }

        public dynamic Set(string name, dynamic value)
        {
            Environment scope = Lookup(name);
            
            // Maybe remove && parent != null
            if (scope == null)
            {
                Define(name, value);
                //throw new Exception("Undefined variable " + name);
            }

            Environment env = scope == null ? this : scope;
            env.privateVars[name] = value;
            return env.vars[name] = value;
        }

        public void Define(string name, dynamic value)
        {
            this.privateVars[name] = value;
            this.vars[name] = value;
        }
    }
}