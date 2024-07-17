
import './imask.js';

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
export function dateMask(elementId)
{
    IMask(document.getElementById(elementId), {
        mask: 'YYYY/MM/DD',        
        lazy: false, 
        placeholderChar: '_',
        displayChar: '',
        blocks: {
            DD: {
                mask: IMask.MaskedRange,
                from: 1,
                to: 31,
                maxLength: 2,
            },
            MM: {
                mask: IMask.MaskedRange,
                from: 1,
                to: 12,
                maxLength: 2,
            },
            YYYY: {
                mask: IMask.MaskedRange,
                from: 1300,
                to: 1500,
            }
        },
    })
}

