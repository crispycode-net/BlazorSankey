export function getDimensions(element) {
    const rect = element.getBoundingClientRect();
    return { width: rect.width, height: rect.height };
};