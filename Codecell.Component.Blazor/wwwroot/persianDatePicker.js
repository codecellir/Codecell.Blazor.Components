
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
                from: 1200,
                to: 1800,
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
    const maskInstance = masks.get(elementId);
    if (maskInstance) {
        maskInstance.reset();
    } else {
        console.warn(`No mask instance found for ${elementId}`);
    }
}  
export function getValue(elementId)
{
    return document.getElementById(elementId).value;
}


export function getElementPosition(childSelector, parentSelector) {
    const child = document.querySelector(childSelector);
    const parent = document.querySelector(parentSelector);
    if (!child || !parent) return null;

    const childRect = child.getBoundingClientRect();
    const parentRect = parent.getBoundingClientRect();

    return {
        top: childRect.top - parentRect.top,
        left: childRect.left - parentRect.left,
        width: childRect.width,
        height: childRect.height
    };
}


