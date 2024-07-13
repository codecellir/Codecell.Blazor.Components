
const outsideClickHandlers = new Map();

export function addOutSideClickHandler(elementId, dotnetHelper)
{
    const handler = (e) =>
    {
        if (!document.getElementById(elementId).contains(e.target))
        {
            dotnetHelper.invokeMethodAsync("InvokeClickOutside");
        }
    };

    window.addEventListener("click", handler);
    outsideClickHandlers.set(elementId, handler);
}

export function removeOutSideClickHandler(elementId)
{
    const handler = outsideClickHandlers.get(elementId);

    if (handler)
    {
        window.removeEventListener("click", handler);
        outsideClickHandlers.delete(elementId);
    }
}

