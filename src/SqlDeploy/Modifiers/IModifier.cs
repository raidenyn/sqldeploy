using JetBrains.Annotations;

namespace SqlDeploy.Modifiers
{
    internal interface IModifier
    {
        void Modify([NotNull] SqlProject project);
    }
}
