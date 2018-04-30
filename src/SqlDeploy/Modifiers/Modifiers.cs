using System.Collections.Generic;
using JetBrains.Annotations;

namespace SqlDeploy.Modifiers
{
    internal class Modifiers: IModifier
    {
        [NotNull]
        private readonly List<IModifier> _modifiers = new List<IModifier>
        {
            new DeleteDbBeforeCreation(),
            new SetRecoveryMode(),
            new SeparateConstrains(),
        };

        public void Modify(SqlProject project)
        {
            foreach (var modifier in _modifiers)
            {
                modifier.Modify(project);
            }
        }
    }
}
