import { WordComponent } from './word.component';
import { SentenceRow } from './row';
import { PlaceholderComponent } from './placeholders.component';

export interface Sentence {
    id: string;
    begin: number;
    end: number;
    text: string;
    tokens?: any[];
    entities?: any[];
    relations?: any[];
    placeholders?: any;
}

export interface SentenceData {
    sentenceRowList: SentenceRow[];
    blockList?: Block[];
    labelList?: Label[];
    wordList?: WordComponent[];
    labelLineList?: LabelLine[];
    connectLineList?: ConnectLine[];
    placeholderList?: Placeholder[];
    lineList?: Line[];
    relationTextList?: RelationText[]
}

export interface Entity {
    id: string;
    type: string;
    begin: number;
    end: number;
    text: string;
    color: string;
    title: string;
    level: number;
}


export interface Relation {
    id: string;
    args: string[];
    text?: string;
}

export interface RelationText extends Relation{
    text: string;
    left: number;
    top: number;
}

export interface Block extends Entity {
    row: number;
    level: number;
    color: string;
    title: string;
    wordList: WordComponent[];
}

export interface LabelLine {
    left: number;
    top: number;
}

export interface Rect {
    width: number;
    height: number;
    left: number;
    top: number;
}

export interface Word extends Rect {
    index: number;
    text: string;
}

export interface Label extends Rect, Entity {
    row: number;
    wordList: Word[];
    level: number;
    isBegin?: boolean;
    isEnd?: boolean;
}

export interface Placeholder {
    row: number;
    id: string;
    level: number;
    [propName: string]: any;
}

export interface Line extends Rect{
    id: string;
}

export interface ConnectLine {
    level: number;
    row: number;
    id: string;
    begin: number;
    end: number;
    path: string;
    wordList: WordComponent[];
    from: Label | Placeholder;
    to: Label | Placeholder;
    text: string;
}
