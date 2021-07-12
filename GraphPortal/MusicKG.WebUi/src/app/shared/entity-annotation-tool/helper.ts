const LEVEL_HEIGHT = 50;
export function createSentenceRowList(wordComponentList: any[], entityList: any[]) {
    let rowList = [];
    let labelList = [];
    let lineList = [];
    let labelUserList = []
    let topIndex = 0;  
    let rowNumbers = getRowNumbers(wordComponentList);
    rowNumbers.forEach((top: number, index: number) => {
        let sentenceRow: any = {};
        sentenceRow.row = index + 1;
        sentenceRow.wordList = wordComponentList.filter(w => w.hostEl.nativeElement.getBoundingClientRect().top == top);
        if (sentenceRow.wordList && sentenceRow.wordList.length > 0) {
            sentenceRow.start = sentenceRow.wordList[0].index;
            sentenceRow.end = sentenceRow.wordList[sentenceRow.wordList.length - 1].index + 1;
        }
        sentenceRow.blockList = getRowBlockList(entityList, sentenceRow.wordList, sentenceRow.row);
        sentenceRow.labelList = getRowLabelList(sentenceRow.blockList, sentenceRow.wordList, sentenceRow.row);
        rowList.push(sentenceRow);
        labelList.push(...sentenceRow.labelList);
    });
    rowList = rowList.map(row => {
        row.maxLevel = row.labelList.reduce((p, c) => {
            return c.level > p ? c.level : p;
        }, 0);
        row.top = topIndex;
        topIndex += getRowHeight(row);
        row.labelList = row.labelList.map(label => {
            label.top = row.top + (row.maxLevel - label.level) * LEVEL_HEIGHT + (LEVEL_HEIGHT - label.height) / 2;
            return label;
        });
        return row;
    });
    labelList.forEach(label => {
        let data = {
            id: label.id,
            top: label.top + label.height,
            height: label.level * LEVEL_HEIGHT - label.height - (LEVEL_HEIGHT - label.height) / 2,
            width: 2
        };
        if (label.isStart) {
            lineList.push({ ...data, left: label.left });
            if(label.user) {
                labelUserList.push({
                    left: label.left,
                    user: label.user,
                    top: label.top - 18,
                    width: label.width,
                    id: label.id
                })
            }
        }
        if (label.isEnd) {
            lineList.push({ ...data, left: (label.left + label.width - 2) });
        }
    });
    return {
        rowList,
        labelList,
        lineList,
        labelUserList
    };
}

export function getRowHeight(sentenceRow: any) {
    let textHeight: number;
    if (sentenceRow.wordList[0]) {
        let labelMaxLevel = sentenceRow.labelList.reduce((p, c) => {
            return c.level > p ? c.level : p;
        }, 0);
        textHeight = sentenceRow.wordList[0].hostEl.nativeElement.getBoundingClientRect().height + labelMaxLevel * 2
    } else {
        textHeight = sentenceRow.placeholderList[0].height;
    };
    return sentenceRow.maxLevel * LEVEL_HEIGHT + textHeight;
}

export function getRowLabelList(blockList: any[], wordComponentList: any[], rowIndex: number) {
    let rowLabelList = [];
    blockList.forEach((block: any) => {
        rowLabelList.push({
            ...block,
            isStart: block.wordList[0].index == block.start,
            isEnd: block.wordList[block.wordList.length - 1].index == (block.end - 1),
            width: block.wordList.reduce((r, v) => {
                let l = 0;
                let width = v.hostEl.nativeElement.getBoundingClientRect().width;
                let box = v.boxList.find(b => b.id == block.id);
                if (v.index != block.start && v.index != (block.end - 1)) {
                    l += (v.marginLeft + v.marginRight)
                }
                if (v.index == block.start && v.index != (block.end - 1)) {
                    l += (box.left * -1 + v.marginRight);
                }
                if (v.index == (block.end - 1) && v.index != block.start) {
                    l += (box.width - width + v.marginLeft);
                }
                if (v.index == (block.end - 1) && v.index == block.start) {
                    l += (box.width - width);
                }
                l += width;
                return r += l;  
            }, 0),
            height: block.wordList[0].hostEl.nativeElement.getBoundingClientRect().height,
            left: wordComponentList.reduce((r, v) => {
                if (v.index == block.start) {
                    let box = v.boxList.find(b => b.id == block.id);
                    return r += (v.marginLeft + box.left);
                }
                if (v.index < block.start) {
                    return r += (v.hostEl.nativeElement.getBoundingClientRect().width + v.marginLeft + v.marginRight);
                }
                return r;
            }, 0)
        });
    });
    return rowLabelList;
}

// 确定每行block包含的word
export function getRowBlockList(entityList: any[] = [], rowWordComponentList: any[], rowIndex: number) {
    let rowBlockList = [];
    entityList.forEach((entity: any) => {
        let wordList = rowWordComponentList.filter(w => w.index >= entity.start && w.index < entity.end);
        if (wordList && wordList.length > 0) {
            rowBlockList.push({
                ...entity,
                row: rowIndex,
                wordList
            });
        }
    });
    return rowBlockList;
}

// 根据render后的word确定行数
export function getRowNumbers(wordComponentList: any[]) {
    let topSet = new Set(wordComponentList.map(d => d.hostEl.nativeElement.getBoundingClientRect().top));
    return Array.from(topSet);
}

export function getEntityList(entityList: any[] = []) {
    let blockList = [];
    entityList.forEach((entity: any) => {
        let level = 1;
        blockList.forEach((block: any) => {
            if (block.end > entity.start && block.start < entity.end) { 
                level = level > (block.level + 1) ? level : (block.level + 1);
            }
        });
        blockList.push({ ...entity, level });
    });
    return blockList;
}

export function getWordRange(word: any, entityList: any) {
    let entities = entityList.filter(e => word.index >= e.start && word.index < e.end);
    if (entities && entities.length > 0) {
        let entity = entities.sort((a, b) => a.level - b.level)[0];
        return {
            entity: entity.entityType,
            start: entity.start,
            end: entity.end,
            level: entity.level,
            id: entity.id,
            color: entity.color
        };
    }
    return { start: word.index, end: word.index + 1 };
}