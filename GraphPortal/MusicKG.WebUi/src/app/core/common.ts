/**
 * Workspace type for link
 */
export const WOKSPACR_TYPE_LIST = [
    { value: '5c414bd75a01cf00010f9660', annotation: 'textSimilarity' },
    { value: '5c414c0a5a01cf00010f9661', annotation: 'entity' },
    { value: '5c74b10d148dce0714010ed6', annotation: 'tokenization' },
    { value: '5c74b117148dce0714010ed7', annotation: 'textparaphrase' },
    { value: '5d63a6b099b830d028fd436d', annotation: 'dialog' },
    { value: TASK_TYPE.ENTITY, annotation: 'dialog/entity' },
    { value: TASK_TYPE.INTENT, annotation: 'dialog/intent' },
    { value: '5d831ababab4cf767dcaabbf', annotation: 'graph' },
    { value: '5faba9e42f2fb1c4d65e3cc3', annotation: 'classification' },
]

export const isVehicleFailureClassification = (id: string) => {
    const isFind = WOKSPACR_TYPE_LIST.find(type => type.value === id);
    return isFind && isFind.annotation === 'classification';
}

export const enum TASK_TYPE {
    ENTITY = '5d7851d8d6ca8e876f75c431',
    INTENT = '5d78523cd6ca8e876f75c432'
}

export const DEFAULT_OLORS = [
    "#77bbff",
    "#eaaaff",
    "#eaff98",
    "#eeffdd",
    "#ffe6c0",
    "#eeaaee",
    "#99ffec",
    "#ff99ec",
    "#ccebff",
    "#eaaaef",
    "#eaff99",
    "#99ffec",
    "#ff99bb",
    "#eaff99",
    "#33ffec",
    "#ff33ec",
    "#bbebff",
    "#eabbef",
    "#eaff98",
    "#99ffec",
    "#ff99ec",
    "#eaff99",
    "#3eefec",
    "#eaff99",
    "#3eefec",
    "#ff33ec",
    "#ffeb33"
];

/**
 * ROLE MAIN PAGE URL
 */
export const ROLE_MAIN_URL = {
    Administrator: ['/user', '/statistic'],
    Annotator: '/annotator',
    Manager: '/workspace'
};

/**
 * Ontology default color
 */
export const ONTOLOGY_DEFAULT_COLOR = `#ffff00`

/**
 * Text similarity json sechema
 */
export const TEXT_SIMILARITY_SECHEMA = {
    type: 'object',
    properties: {
        Items: {
            type: 'array',
            items: [{
                type: 'object',
                properties: {
                    Id: { type: 'string' },
                    TextSource: { type: 'string' },
                    TextTargets: { type: 'array', itmes: [{ type: 'string' }] }
                },
                required: ['Id', 'TextSource', 'TextTargets'],
                additionalProperties: false
            }]
        },
        Guideline: { type: 'string' }
    },
    additionalProperties: false,
    required: ['Items', 'Guideline']
}

/**
 * Entity json sechema
 */
export const ENTITY_SECHEMA = {
    type: 'object',
    properties: {
        Items: {
            type: 'array',
            items: [{
                type: 'object',
                properties: {
                    Id: { type: 'string' },
                    Text: { type: 'string' },
                    SpanItems: {
                        type: 'array',
                        items: [{
                            type: 'object',
                            properties: {
                                EntityType: { type: 'string' },
                                Start: { type: 'number' },
                                End: { type: 'number' }
                            },
                            required: ['EntityType', 'Start', 'End'],
                            additionalProperties: false
                        }]
                    }
                },
                required: ['Id', 'Text'],
                additionalProperties: false,
            }]
        },
        Guideline: { type: 'string' }
    },
    additionalProperties: false,
    required: ['Items', 'Guideline']
}

/**
 * Tokenization json sechema
 */
export const TOKENIZATION_SECHEMA = {
    type: 'object',
    properties: {
        Items: {
            type: 'array',
            items: [{
                type: 'object',
                properties: {
                    Id: { type: 'string' },
                    Text: { type: 'string' },
                    SpanItems: {
                        type: 'array',
                        items: [{
                            type: 'object',
                            properties: {
                                Start: { type: 'number' },
                                End: { type: 'number' }
                            },
                            required: ['Start', 'End'],
                            additionalProperties: false
                        }]
                    }
                },
                additionalProperties: false,
                required: ['Id', 'Text']
            }]
        },
        Guideline: { type: 'string' }
    },
    additionalProperties: false,
    required: ['Items']
}

/**
 * Text Paraphrase json sechema
 */
export const TEXT_PARAPHRASE_SECHEMA = {
    type: 'object',
    properties: {
        Items: {
            type: 'array',
            items: [{
                type: 'object',
                properties: {
                    Id: { type: 'string' },
                    Subject: { type: 'string' },
                    Predicate: { type: 'string' },
                    Object: { type: 'string' },
                    ExampleQuestions: { type: 'array', itmes: [{ type: 'string' }] }
                },
                required: ['Id', 'Subject', 'Predicate', 'Object', 'ExampleQuestions'],
                additionalProperties: false
            }]
        },
        Guideline: { type: 'string' }
    },
    additionalProperties: false,
    required: ['Items', 'Guideline']
}

/**
 * Workspace type for export csv
 */
export const WORKSPCE_LIST_CSV = [
    { value: '5c414bd75a01cf00010f9660', annotation: 'textSimilarity' },
    { value: '5c414c0a5a01cf00010f9661', annotation: 'entity' }
]