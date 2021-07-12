import { Word, Label, Block, Placeholder, ConnectLine, Sentence, Entity, LabelLine, Relation, SentenceData, Rect, Line, RelationText } from './interface';
import { WordComponent } from './word.component';
import { PlaceholderComponent } from './placeholders.component';

const LEVEL_HEIGHT = 50;

export function createSentenceRowList(sentence: Sentence, wordComponentList: WordComponent[], placeholderComponentList: PlaceholderComponent[], sentenceEl: HTMLElement): SentenceData {
    let sentenceRowList: SentenceRow[] = [];
    let labelList: Label[] = [];
    let placeholderList: Placeholder[] = [];
    let connectLineList: ConnectLine[] = [];
    let lineList: Line[] = [];
    let relationTextList: RelationText[] = [];
    let topIndex = 5;
    let rowNumbers = getRowNumbers(wordComponentList, placeholderComponentList);
    rowNumbers.forEach((top: number, index: number) => {
        let sentenceRow = new SentenceRow();
        sentenceRow.row = index + 1;
        sentenceRow.wordList = wordComponentList.filter(w => w.hostEl.nativeElement.getBoundingClientRect().top == top);
        if (sentenceRow.wordList && sentenceRow.wordList.length > 0) {
            sentenceRow.begin = sentenceRow.wordList[0].index;
            sentenceRow.end = sentenceRow.wordList[sentenceRow.wordList.length - 1].index + 1;
        }
        sentenceRow.placeholderList = placeholderComponentList.filter(p => p.hostEl.nativeElement.getBoundingClientRect().top == top).map(p => {
            let rect = p.hostEl.nativeElement.getBoundingClientRect();
            return {
                level: 0,
                row: sentenceRow.row,
                id: p.id,
                width: rect.width,
                left: rect.left - sentenceEl.getBoundingClientRect().left,
                height: rect.height,
            };
        });
        sentenceRow.blockList = getRowBlockList(sentence.entities, sentenceRow.wordList, sentenceRow.row);
        sentenceRow.labelList = getRowLabelList(sentenceRow.blockList, sentenceRow.wordList, sentenceRow.row);
        sentenceRowList.push(sentenceRow);
        labelList.push(...sentenceRow.labelList);
        placeholderList.push(...sentenceRow.placeholderList);
    });
    connectLineList = getRowConnectLineList(sentence.relations, labelList, placeholderList);
    sentenceRowList = sentenceRowList.map(row => {
        row.maxLevel = getRowMaxLevel(row.row, row.labelList, connectLineList);
        row.top = topIndex;
        topIndex += getRowHeight(row);
        row.labelList = row.labelList.map(label => {
            label.top = row.top + (row.maxLevel - label.level) * LEVEL_HEIGHT + (LEVEL_HEIGHT - label.height) / 2;
            return label;
        });
        row.placeholderList = row.placeholderList.map(placeholder => {
            placeholder.top = row.top + row.maxLevel * LEVEL_HEIGHT;
            return placeholder;
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
        if (label.isBegin) {
            lineList.push({...data,left: label.left});
        }
        if (label.isEnd) {
            lineList.push({...data,left: (label.left + label.width - 2)});
        }
    });
    connectLineList = connectLineList.map(connect => {
        let fromMid = connect.from.left + connect.from.width / 2;
        let toMid = connect.to.left + connect.to.width / 2;
        let row = sentenceRowList.find(row => row.row == connect.row);
        let upY = row.top + (row.maxLevel - connect.level) * LEVEL_HEIGHT + LEVEL_HEIGHT * 0.7;
        connect.path = `
            M ${fromMid - 10} ${connect.from.top}
            L ${fromMid > toMid ? (fromMid - 20) : (fromMid)} ${upY}
            L ${fromMid > toMid ? (toMid + 10) : (toMid - 10)} ${upY}
            L ${toMid} ${connect.to.top - 2.5}
        `;
        relationTextList.push({
            left: (fromMid + toMid - 20) / 2 - connect.text.length * 7,
            top: upY - 20,
            id: connect.id,
            text: connect.text,
            args: []
        });
        return connect;
    });
    return {
        sentenceRowList,
        labelList,
        placeholderList,
        connectLineList,
        lineList,
        relationTextList
    };
}

export function getRowHeight(sentenceRow: SentenceRow) {
    let textHeight: number;
    if(sentenceRow.wordList[0]) {
        let labelMaxLevel = sentenceRow.labelList.reduce((p, c) => {
            return c.level > p ? c.level : p;
        }, 0);
        textHeight = sentenceRow.wordList[0].hostEl.nativeElement.getBoundingClientRect().height + labelMaxLevel * 2
    }else {       
        textHeight = sentenceRow.placeholderList[0].height;
    };
    return sentenceRow.maxLevel * LEVEL_HEIGHT + textHeight;
}

export function getRowMaxLevel(rowIndex: number, labelList: Label[], connectLineList: ConnectLine[]) {
    let labelMaxLevel = labelList.reduce((p, c) => {
        return c.level > p ? c.level : p;
    }, 0);
    let connectMaxLevel = connectLineList.filter(connect => connect.row == rowIndex).reduce((p, c) => {
        return c.level > p ? c.level : p;
    }, 0);
    return Math.max(labelMaxLevel, connectMaxLevel);
}

export function getRowConnectLineList(relationList: Relation[] = [], labelList: Label[], placeholderList: Placeholder[]) {
    let connectTemp = [];
    let connectLineList = [];
    relationList.forEach(relation => {
        let fromLabel = labelList.find(l => l.id == relation.args[0] && l.isBegin);
        let fromPlaceholder = placeholderList.find(p => p.id == relation.args[0]);
        let toLabel = labelList.find(l => l.id == relation.args[1] && l.isBegin);
        let toPlaceholder = placeholderList.find(p => p.id == relation.args[1]);
        let from: any = fromLabel || fromPlaceholder;
        let to: any = toLabel || toPlaceholder;
        let relationLevel = from.row < to.row ? from.level : (from.row > to.row ? to.level : Math.max(from.level, to.level));
        let fromMid = from.left + from.width / 2;
        let toMid = to.left + to.width / 2;
        connectTemp.push({
            id: relation.id,
            row: Math.min(from.row, to.row),
            level: relationLevel + 1,
            begin: fromMid < toMid ? fromMid : toMid,
            end: fromMid < toMid ? (toMid - 10) : (fromMid + 10),
            from: from,
            to: to,
            text: relation.text
        });
    });
    return connectTemp.map(connect => {
        let connectLine = getConnectLevel(connect, connectLineList, labelList);
        connectLineList.push(connectLine);
        return connectLine;
    });
}

export function getConnectLevel(connectLine: ConnectLine, connectList: ConnectLine[], labelList: Label[]) {
    let islabelX = labelList.find(l => l.left <= connectLine.end && (l.left + l.width) >= connectLine.begin && l.level == connectLine.level && l.row == connectLine.row);
    let islineX = connectList.find(l => l.begin <= connectLine.end && l.end >= connectLine.begin && l.level == connectLine.level && l.row == connectLine.row);
    if (islabelX || islineX) {
        connectLine.level += 1;
        return getConnectLevel(connectLine, connectList, labelList);
    } else {
        return connectLine;
    }
}

// 根据render后的word和placeholder确定行数
export function getRowNumbers(wordComponentList: WordComponent[], placeholderComponentList: PlaceholderComponent[]) {
    let domList = [].concat(wordComponentList, placeholderComponentList).map(c => c.hostEl.nativeElement);
    let topSet = new Set(domList.map(d => d.getBoundingClientRect().top));
    return Array.from(topSet);
}

// 确定每行block包含的word
export function getRowBlockList(entityList: Entity[] = [], rowWordComponentList: WordComponent[], rowIndex: number) {
    let rowBlockList = [];
    entityList.forEach((entity: Entity) => {
        let wordList = rowWordComponentList.filter(w => w.index >= entity.begin && w.index < entity.end);
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

export function getRowLabelList(blockList: Block[], wordComponentList: WordComponent[], rowIndex: number) {
    let rowLabelList = [];
    blockList.forEach((block: Block) => {
        let level = 1;
        rowLabelList.forEach((label: Label) => {
            if (label.end > block.begin && label.begin < block.end) {
                level = label.level + 1;
            }
        });
        rowLabelList.push({
            ...block,
            level,
            isBegin: block.wordList[0].index == block.begin,
            isEnd: block.wordList[block.wordList.length - 1].index == (block.end - 1),
            width: block.wordList.reduce((r, v) => {
                let l = 0;
                let width = v.hostEl.nativeElement.getBoundingClientRect().width;
                let box = v.boxList.find(b => b.entity == block.id);
                if (v.index != block.begin && v.index != (block.end - 1)) {
                    l += (v.marginLeft + v.marginRight)
                }
                if (v.index == block.begin && v.index != (block.end - 1)) {
                    l += (box.left * -1 + v.marginRight);
                }
                if (v.index == (block.end - 1) && v.index != block.begin) {
                    l += (box.width - width + v.marginLeft);
                }
                if (v.index == (block.end - 1) && v.index == block.begin) {
                    l += (box.width - width);
                }
                l += width;
                return r += l;
            }, 0),
            height: block.wordList[0].hostEl.nativeElement.getBoundingClientRect().height,
            left: wordComponentList.reduce((r, v) => {
                if (v.index == block.begin) {
                    let box = v.boxList.find(b => b.entity == block.id);
                    return r += (v.marginLeft + box.left);
                }
                if (v.index < block.begin) {
                    return r += (v.hostEl.nativeElement.getBoundingClientRect().width + v.marginLeft + v.marginRight);
                }
                return r;
            }, 0)
        });
    });
    return rowLabelList;
}

// 根据entities确定block的层级关系
export function getBlockList(entityList: Entity[] = []) {
    let blockList = [];
    entityList.forEach((entity: Entity) => {
        let level = 1;
        blockList.forEach((block: Block) => {
            if (block.end > entity.begin && block.begin < entity.end) {
                level = block.level + 1;
            }
        });
        blockList.push({ ...entity, level });
    });
    return blockList;
}

export class SentenceRow {
    row: number;
    maxLevel: number;
    top: number;
    wordList: WordComponent[];
    labelList: Label[];
    blockList: Block[];
    placeholderList: Placeholder[];
    connectLineList: ConnectLine[];
    lineList: Line[];
    begin: number;
    end: number;

    constructor() { }

}