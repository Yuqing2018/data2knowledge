export function parseEntity(entityList: any) {
    let labelList = [];
    if (entityList) {
        for (let entity of entityList) {
            let level = 1;
            for (let label of labelList) {
                if (entity.end > label.begin && entity.begin < label.end) {
                    level = level > (label.level + 1) ? level : (label.level + 1);
                }
            }
            entity.level = level;
            labelList.push(entity);
        }
    }
    return labelList;
}

export function getWordRange(word: any, entityList: any, tokenList: any) {
    let entities = entityList.filter(e => word.index >= e.begin && word.index < e.end);
    if (entities && entities.length > 0) {
        let entity = entities.sort((a, b) => a.level - b.level)[0];
        return {
            entity: entity.id,
            begin: entity.begin,
            end: entity.end,
            level: entity.level
        };
    } else {
        let token = tokenList.find(t => word.index >= t.begin && word.index < t.end)
        return {
            begin: token.begin,
            end: token.end
        }
    }
}

/**
 * 计算二阶贝塞尔曲线的控制点
 * @param  sx     起点x坐标
 * @param  sy     起点y坐标
 * @param  dx     终点x坐标
 * @param  dy     终点y坐标
 * @return point  控制点坐标 
 */
export function calControlPoint(sx,sy,dx,dy){
    var a,x,y,X,Y,len;
    X = (sx + dx) / 2;  
    Y = (sy + dy) / 2;
    len = 0.1 * Math.sqrt(Math.pow((dy - sy),2) + Math.pow((dx - sx),2)); // 控制贝塞尔曲线曲率
    a = Math.atan2(dy - sy, dx - sx);
    return {x: X - len * Math.sin(a),y: Y + len * Math.cos(a)}
}