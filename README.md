# Atropos
本人毕设，unity+c#实现的一个像素风解谜游戏，解谜部分是复刻了编程游戏TIS-100，整个项目实现了主要的功能模块。可以直接使用现有的模块制作一个新的剧情。

整个解谜模式的程序使用了解释器模式来设计。玩家需要在解谜模式中的编程单元内编程，根据要求输出正确答案就算成功，如果失败了可以继续尝试。<br>
解谜模式中有12个编程单元，他们可与上下左右相邻的单元通信，每个玩家可以在其中编写程序计算或者与相邻单元进行通信。程序执行过程会从第一条指令开始逐条执行。当执行完单元中程序的最后一条指令后，会自动返回第一条指令继续执行。<br>

## 端口和寄存器介绍：<br>
ACC <br>
•	类型：寄存器<br>
•	描述：ACC是编程单元的基本存储寄存器。ACC被用作隐式源或许多指令的目标操作数，如算术指令和条件指令，初值为0。<br>

BAK<br>
•	类型：寄存器<br>
•	描述：BAK是ACC中的值的临时存储器。它仅可通过SAV或SWP访问，不能直接读写，初值为0。<br>

NIL<br>
•	类型：寄存器（特殊）<br>
•	描述：从NIL中读会产生0值。向NIL中写不会产生任何效果。如果你想舍弃一条指令的结果，可以使用NIL作为目标操作数。<br>

LEFT，RIGHT，UP，DOWN<br>
•	类型：端口<br>
•	描述：LEFT，RIGHT，UP，DOWN为4个通信寄存器，他们也代表了所有基本处理单元用来与其拓扑相邻单元通信的4个端口。当执行到一条读写指令时，再完成读写之前该单元的程序都不会继续执行。<br>

ANY<br>
•	类型：端口（伪端口）<br>
•	描述：可以表示任意端口，指令会首先对任意可用的端口进行读写。<br>

## 指令介绍：<br>
标签<br>
•	语法：<LABEL>:<br>
•	描述：标签被用来辨识跳转指令的目标地。当一个标签被用来作为跳转目标时，标签后的指令将会接着被执行。<br>
•	示例：<br>
•	 LOOP: # 这个标签独自占据一行<br>
•  L: MOV 8, ACC # 这个标签和一条指令共用一行<br>

MOV<br>
•	语法：MOV <参数1> <参数2><br>
•	描述：<参数1>将会被读取，返回值会被写入<参数2>。<br>
•	示例：<br>
•	 MOV 8, ACC       # 常值8被写入ACC寄存器<br>
•	 MOV LEFT, RIGHT  # LEFT中的值被读出然后写入RIGHT<br>
•    MOV UP, NIL      # UP中的值被读出然后被舍弃<br>

SWP<br>
•	语法：SWP<br>
•	描述：交换ACC和BAK中的值<br>

SAV<br>
•	语法：SAV<br>
•	描述：将ACC中值写入BAK<br>

ADD<br>
•	语法：ADD <参数><br>
•	描述：<参数>中的值与ACC相加，结果重写写入ACC<br>
•	示例：<br>
•	 ADD 16           # 常值16与ACC相加，结果重写写入ACC<br>
•    ADD LEFT         # LEFT中的值被读取并与ACC相加，结果重写写入ACC<br>

SUB<br>
•	语法：SUB <参数><br>
•	描述：ACC减去<参数>中的值，结果重写写入ACC<br>
•	示例：<br>
•	 SUB 16           # ACC减去常值16，结果重写写入ACC<br>
•    SUB LEFT         # ACC减去LEFT中的值，结果重写写入ACC<br>

NEG<br>
•	语法：NEG<br>
•	描述：对ACC取算数相反数并存入ACC，0值保持不变。<br>

JMP<br>
•	语法：JMP <LABEL><br>
•	描述：无条件跳转指令。标签<LABEL>后的指令会紧接着被执行。<br>

JEZ<br>
•	语法：JEZ <LABEL><br>
•	描述：条件跳转指令。如果ACC为0，标签<LABEL>后的指令会紧接着被执行。<br>

JNZ<br>
•	语法：JNZ <LABEL><br>
•	描述：条件跳转指令。如果ACC不为0，标签<LABEL>后的指令会紧接着被执行。<br>

JGZ<br>
•	语法：JGZ <LABEL><br>
•	描述：条件跳转指令。如果ACC大于0，标签<LABEL>后的指令会紧接着被执行。<br>

JLZ<br>
•	语法：JLZ <LABEL><br>
•	描述：条件跳转指令。如果ACC小于0，标签<LABEL>后的指令会紧接着被执行。<br>

JRO<br>
•	语法：JRO <参数><br>
•	描述：无条件跳转指令。会跳转到<参数>指定的偏移量处执行。<br>


## 文法<br>
G=(VT,VN,S,P)如下<br>
1.	VT={ACC,BAK,NIL,LEFT,RIGHT,UP,DOWN,ANY,MOV,SWP,SAV,ADD,SUB,NEG,JMP,JEZ,JNZ,JGZ,JLZ,JRO,x,i,:}<br>
2.	VN={<表达式>,<标签>,<指令>,<源指令>,<参数>}<br>
3.	S=<表达式><br>
4.	P={<br>
   <表达式>→ <标签>:<指令>|<指令>|<标签>:<br>
   <指令>  → <源指令> <参数><br>
   <源指令>→ MOV <参数>|ADD|SUB|JRO|SAV|SWP|NEG|JMP|JEZ|JNZ|JGZ|JLZ|JRO<br>
   <参数>  → <标签>|LEFT|RIGHT|UP|DOWN|ANY|ACC|NIL|x<br>
   <标签>  →i<br>
   }<br>
其中x是-999到999之间的整数。

