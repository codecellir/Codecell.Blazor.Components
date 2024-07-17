
import './imask.js';

const outsideClickHandlers = new Map();
const masks = new Map();

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
export function dateMask(elementId, dotnetHelper)
{
    var mask = IMask(document.getElementById(elementId), {
        mask: 'YYYY/MM/DD',
        lazy: false,
        placeholderChar: '_',  
        blocks: {
            YYYY: {
                mask: IMask.MaskedRange,
                from: 1300,
                to: 1500,
            }  ,
            MM: {
                mask: IMask.MaskedRange,
                from: 1,
                to: 12,
                maxLength: 2,
            },
            DD: {
                mask: IMask.MaskedRange,
                from: 1,
                to: 31,
                maxLength: 2,
            },
        },
    })

    mask.on('complete', () =>
    {
        dotnetHelper.invokeMethodAsync("InvokeMakComplete");
    });

    masks.set(elementId, mask);
}

export function resetMask(elementId)
{
    const mask = masks.get(elementId);

    if (mask)
    {
        mask.reset();
    }
}  
export function getValue(elementId)
{
    return document.getElementById(elementId).value;
}

