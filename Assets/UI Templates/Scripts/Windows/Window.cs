namespace RemoteEducation.UI
{

    /// <summary>
    ///     A specialization of the <see cref="Toggle"/> component which
    ///     registers itself with the active <see cref="WindowManager"/> 
    ///     in order to track open windows
    /// </summary>
    public class Window : Toggle
    {

        /// <summary>
        ///		Indicates whether this Window should be closeable by the active WindowManager 
        ///		when it is the topmost Window
        /// </summary>
        public bool Closeable = true;

        /// <summary>
        ///     When the Window is turned on, it is added to the ordered list of Windows
        ///     tracked by the <see cref="WindowManager"/> 
        /// </summary>
        protected override void TurnOnImmediate()
        {
            base.TurnOnImmediate();

            if (Closeable)
            {
                WindowManager.Register(this);
            }
        }


        /// <summary>
        ///     Overrides the base TurnOffSiblingsNow from <see cref="Toggle"/> to add
        ///     consideration for the <see cref="Closeable"/> flag
        /// </summary>
        public override void TurnOffSiblingsNow()
        {
            // Windows which are top-level GameObjects will not consider sibling objects
            if (!transform.parent) { return; }

            var ignore = transform.GetSiblingIndex();

            for (var idx = transform.parent.childCount - 1; idx >= 0; idx--)
            {
                if (idx == ignore) { continue; }

                var sibling = transform.parent.GetChild(idx).GetComponent<Window>();

                if (sibling && sibling.Closeable)
                {
                    sibling.TurnOff();
                }
            }
        }
    }
}