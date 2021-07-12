
interface Node {
    Id: string;
    ParentId?: string;
    Children?: Node[];
    [propName: string]: any
}

export function unflatten(array: Node[], parent?: Node, tree?: Node[]) {
    tree = tree || [];
    let children = array.filter(child => parent ? child.ParentId == parent.Id : !child.ParentId);
    if (children && children.length > 0) {
        if (!parent) {
            tree = children;
        } else {
            parent.Children = children
        }
        for (let child of children) {
            unflatten(array, child)
        }
    }
    return tree;
}

export function getRandomColor() {
    let randomColor = '#000000';
    while (chroma.contrast(randomColor, '#000000') < 4.8) {
        randomColor = chroma.random().toString()
    }
    return randomColor;
}

export function uuid(): string {
    let s = [];
    let hexDigits = "0123456789abcdef";
    for (let i = 0; i < 36; i++) {
        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
    }
    s[14] = "4";
    s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);
    s[8] = s[13] = s[18] = s[23] = "-";
    let uuid = s.join("");
    return uuid;
}