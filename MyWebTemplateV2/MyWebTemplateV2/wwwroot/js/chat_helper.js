window.chatHelper = {
    renderMath: function (elementId) {
        var el = document.getElementById(elementId);
        if (el && typeof renderMathInElement === 'function') {
            renderMathInElement(el, {
                delimiters: [
                    { left: '$$', right: '$$', display: true },
                    { left: '$', right: '$', display: false },
                    { left: '\\(', right: '\\)', display: false },
                    { left: '\\begin{equation}', right: '\\end{equation}', display: true },
                    { left: '\\begin{align}', right: '\\end{align}', display: true },
                    { left: '\\begin{alignat}', right: '\\end{alignat}', display: true },
                    { left: '\\begin{gather}', right: '\\end{gather}', display: true },
                    { left: '\\begin{CD}', right: '\\end{CD}', display: true },
                    { left: '\\[', right: '\\]', display: true }
                ],
                throwOnError: false
            });
        }
    },
    scrollToBottom: function (elementId) {
        var el = document.getElementById(elementId);
        if (el) {
            el.scrollTop = el.scrollHeight;
        }
    }
};
