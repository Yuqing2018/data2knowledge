import { TextEllipsisPipe } from './text-ellipsis.pipe';

describe('TextEllipsisPipe', () => {
  const pipe = new TextEllipsisPipe();

  it('create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('transforms "长度过长长度过长长度过长长度过长" to "长度过长长度过长长度过长长..."', () => {
    expect(pipe.transform('长度过长长度过长长度过长长度过长')).toBe('长度过长长度过长长度过长长...');
  });

  it('transforms "长度正常" to "长度正常"', () => {
    expect(pipe.transform('长度正常')).toBe('长度正常');
  });

  it('transforms "指定长度" to "指定..."', () => {
    expect(pipe.transform('指定长度',[2])).toBe('指定...');
  });

});
