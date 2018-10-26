//protected override void Go() {
//            Goto(0);

//            for (;;) {
//#if DBG
//                if (runmatch.Debug) {
//                    DumpState();
//                }
//#endif

//                CheckTimeout();

//                switch (Operator()) {
//                    case RegexCode.Stop:
//                        return;

//                    case RegexCode.Nothing:
//                        break;

//                    case RegexCode.Goto:
//                        Goto(Operand(0));
//                        continue;

//                    case RegexCode.Testref:
//                        if (!IsMatched(Operand(0)))
//                            break;
//                        Advance(1);
//                        continue;

//                    case RegexCode.Lazybranch:
//                        TrackPush(Textpos());
//                        Advance(1);
//                        continue;

//                    case RegexCode.Lazybranch | RegexCode.Back:
//                        TrackPop();
//                        Textto(TrackPeek());
//                        Goto(Operand(0));
//                        continue;

//                    case RegexCode.Setmark:
//                        StackPush(Textpos());
//                        TrackPush();
//                        Advance();
//                        continue;

//                    case RegexCode.Nullmark:
//                        StackPush(-1);
//                        TrackPush();
//                        Advance();
//                        continue;

//                    case RegexCode.Setmark | RegexCode.Back:
//                    case RegexCode.Nullmark | RegexCode.Back:
//                        StackPop();
//                        break;

//                    case RegexCode.Getmark:
//                        StackPop();
//                        TrackPush(StackPeek());
//                        Textto(StackPeek());
//                        Advance();
//                        continue;

//                    case RegexCode.Getmark | RegexCode.Back:
//                        TrackPop();
//                        StackPush(TrackPeek());
//                        break;

//                    case RegexCode.Capturemark:
//                        if (Operand(1) != -1 && !IsMatched(Operand(1)))
//                            break;
//                        StackPop();
//                        if (Operand(1) != -1)
//                            TransferCapture(Operand(0), Operand(1), StackPeek(), Textpos());
//                        else
//                            Capture(Operand(0), StackPeek(), Textpos());
//                        TrackPush(StackPeek());

//                        Advance(2);

//                        continue;

//                    case RegexCode.Capturemark | RegexCode.Back:
//                        TrackPop();
//                        StackPush(TrackPeek());
//                        Uncapture();
//                        if (Operand(0) != -1 && Operand(1) != -1)
//                            Uncapture();

//                        break;

//                    case RegexCode.Branchmark:
//                        {
//                            int matched;
//                            StackPop();

//                            matched = Textpos() - StackPeek();

//                            if (matched != 0) {                     // Nonempty match -> loop now
//                                TrackPush(StackPeek(), Textpos());  // Save old mark, textpos
//                                StackPush(Textpos());               // Make new mark
//                                Goto(Operand(0));                   // Loop
//                            }
//                            else {                                  // Empty match -> straight now
//                                TrackPush2(StackPeek());            // Save old mark
//                                Advance(1);                         // Straight
//                            }
//                            continue;
//                        }

//                    case RegexCode.Branchmark | RegexCode.Back:
//                        TrackPop(2);
//                        StackPop();
//                        Textto(TrackPeek(1));                       // Recall position
//                        TrackPush2(TrackPeek());                    // Save old mark
//                        Advance(1);                                 // Straight
//                        continue;

//                    case RegexCode.Branchmark | RegexCode.Back2:
//                        TrackPop();
//                        StackPush(TrackPeek());                     // Recall old mark
//                        break;                                      // Backtrack

//                    case RegexCode.Lazybranchmark:
//                        {
//                            // We hit this the first time through a lazy loop and after each 
//                            // successful match of the inner expression.  It simply continues
//                            // on and doesn't loop. 
//                            StackPop();

//                            int oldMarkPos = StackPeek();

//                            if (Textpos() != oldMarkPos) {              // Nonempty match -> try to loop again by going to 'back' state
//                                if (oldMarkPos != -1)
//                                    TrackPush(oldMarkPos, Textpos());   // Save old mark, textpos
//                                else
//                                    TrackPush(Textpos(), Textpos());   
//                            }
//                            else {
//                                // The inner expression found an empty match, so we'll go directly to 'back2' if we
//                                // backtrack.  In this case, we need to push something on the stack, since back2 pops.
//                                // However, in the case of ()+? or similar, this empty match may be legitimate, so push the text 
//                                // position associated with that empty match.
//                                StackPush(oldMarkPos);

//                                TrackPush2(StackPeek());                // Save old mark
//                            }
//                            Advance(1);
//                            continue;
//                        }

//                    case RegexCode.Lazybranchmark | RegexCode.Back:
//                        {
//                            // After the first time, Lazybranchmark | RegexCode.Back occurs
//                            // with each iteration of the loop, and therefore with every attempted
//                            // match of the inner expression.  We'll try to match the inner expression, 
//                            // then go back to Lazybranchmark if successful.  If the inner expression 
//                            // failes, we go to Lazybranchmark | RegexCode.Back2
//                            int pos;

//                            TrackPop(2);
//                            pos = TrackPeek(1);
//                            TrackPush2(TrackPeek());                // Save old mark
//                            StackPush(pos);                         // Make new mark
//                            Textto(pos);                            // Recall position
//                            Goto(Operand(0));                       // Loop
//                            continue;
//                        }

//                    case RegexCode.Lazybranchmark | RegexCode.Back2:
//                        // The lazy loop has failed.  We'll do a true backtrack and 
//                        // start over before the lazy loop. 
//                        StackPop();
//                        TrackPop();
//                        StackPush(TrackPeek());                      // Recall old mark
//                        break;

//                    case RegexCode.Setcount:
//                        StackPush(Textpos(), Operand(0));
//                        TrackPush();
//                        Advance(1);
//                        continue;

//                    case RegexCode.Nullcount:
//                        StackPush(-1, Operand(0));
//                        TrackPush();
//                        Advance(1);
//                        continue;

//                    case RegexCode.Setcount | RegexCode.Back:
//                        StackPop(2);
//                        break;

//                    case RegexCode.Nullcount | RegexCode.Back:
//                        StackPop(2);
//                        break;

//                    case RegexCode.Branchcount:
//                        // StackPush:
//                        //  0: Mark
//                        //  1: Count
//                        {
//                            StackPop(2);
//                            int mark = StackPeek();
//                            int count = StackPeek(1);
//                            int matched = Textpos() - mark;

//                            if (count >= Operand(1) || (matched == 0 && count >= 0)) {                                   // Max loops or empty match -> straight now
//                                TrackPush2(mark, count);            // Save old mark, count
//                                Advance(2);                         // Straight
//                            }
//                            else {                                  // Nonempty match -> count+loop now
//                                TrackPush(mark);                    // remember mark
//                                StackPush(Textpos(), count + 1);    // Make new mark, incr count
//                                Goto(Operand(0));                   // Loop
//                            }
//                            continue;
//                        }

//                    case RegexCode.Branchcount | RegexCode.Back:
//                        // TrackPush:
//                        //  0: Previous mark
//                        // StackPush:
//                        //  0: Mark (= current pos, discarded)
//                        //  1: Count
//                        TrackPop();
//                        StackPop(2);
//                        if (StackPeek(1) > 0) {                         // Positive -> can go straight
//                            Textto(StackPeek());                        // Zap to mark
//                            TrackPush2(TrackPeek(), StackPeek(1) - 1);  // Save old mark, old count
//                            Advance(2);                                 // Straight
//                            continue;
//                        }
//                        StackPush(TrackPeek(), StackPeek(1) - 1);       // recall old mark, old count
//                        break;

//                    case RegexCode.Branchcount | RegexCode.Back2:
//                        // TrackPush:
//                        //  0: Previous mark
//                        //  1: Previous count
//                        TrackPop(2);
//                        StackPush(TrackPeek(), TrackPeek(1));           // Recall old mark, old count
//                        break;                                          // Backtrack


//                    case RegexCode.Lazybranchcount:
//                        // StackPush:
//                        //  0: Mark
//                        //  1: Count
//                        {
//                            StackPop(2);
//                            int mark = StackPeek();
//                            int count = StackPeek(1);

//                            if (count < 0) {                        // Negative count -> loop now
//                                TrackPush2(mark);                   // Save old mark
//                                StackPush(Textpos(), count + 1);    // Make new mark, incr count
//                                Goto(Operand(0));                   // Loop
//                            }
//                            else {                                  // Nonneg count -> straight now
//                                TrackPush(mark, count, Textpos());  // Save mark, count, position
//                                Advance(2);                         // Straight
//                            }
//                            continue;
//                        }

//                    case RegexCode.Lazybranchcount | RegexCode.Back:
//                        // TrackPush:
//                        //  0: Mark
//                        //  1: Count
//                        //  2: Textpos
//                        {
//                            TrackPop(3);
//                            int mark = TrackPeek();
//                            int textpos = TrackPeek(2);

//                            if (TrackPeek(1) < Operand(1) && textpos != mark) { // Under limit and not empty match -> loop
//                                Textto(textpos);                            // Recall position
//                                StackPush(textpos, TrackPeek(1) + 1);       // Make new mark, incr count
//                                TrackPush2(mark);                           // Save old mark
//                                Goto(Operand(0));                           // Loop
//                                continue;
//                            }
//                            else {                                          // Max loops or empty match -> backtrack
//                                StackPush(TrackPeek(), TrackPeek(1));       // Recall old mark, count
//                                break;                                      // backtrack
//                            }
//                        }

//                    case RegexCode.Lazybranchcount | RegexCode.Back2:
//                        // TrackPush:
//                        //  0: Previous mark
//                        // StackPush:
//                        //  0: Mark (== current pos, discarded)
//                        //  1: Count
//                        TrackPop();
//                        StackPop(2);
//                        StackPush(TrackPeek(), StackPeek(1) - 1);   // Recall old mark, count
//                        break;                                      // Backtrack

//                    case RegexCode.Setjump:
//                        StackPush(Trackpos(), Crawlpos());
//                        TrackPush();
//                        Advance();
//                        continue;

//                    case RegexCode.Setjump | RegexCode.Back:
//                        StackPop(2);
//                        break;

//                    case RegexCode.Backjump:
//                        // StackPush:
//                        //  0: Saved trackpos
//                        //  1: Crawlpos
//                        StackPop(2);
//                        Trackto(StackPeek());

//                        while (Crawlpos() != StackPeek(1))
//                            Uncapture();

//                        break;

//                    case RegexCode.Forejump:
//                        // StackPush:
//                        //  0: Saved trackpos
//                        //  1: Crawlpos
//                        StackPop(2);
//                        Trackto(StackPeek());
//                        TrackPush(StackPeek(1));
//                        Advance();
//                        continue;

//                    case RegexCode.Forejump | RegexCode.Back:
//                        // TrackPush:
//                        //  0: Crawlpos
//                        TrackPop();

//                        while (Crawlpos() != TrackPeek())
//                            Uncapture();

//                        break;

//                    case RegexCode.Bol:  //一位操作码
//                        if (Leftchars() > 0 && CharAt(Textpos() - 1) != '\n')   //* 左边存在字符且不等于换行符
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.Eol:     //一位操作码
//                        if (Rightchars() > 0 && CharAt(Textpos()) != '\n')      //* 右边存在字符且不等于换行符
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.Boundary:    //一位操作码
//                        if (!IsBoundary(Textpos(), runtextbeg, runtextend))     //*判断是否为单词边界
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.Nonboundary:     //一位操作码
//                        if (IsBoundary(Textpos(), runtextbeg, runtextend))      //*判断是否为单词边界
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.ECMABoundary:
//                        if (!IsECMABoundary(Textpos(), runtextbeg, runtextend))
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.NonECMABoundary:
//                        if (IsECMABoundary(Textpos(), runtextbeg, runtextend))
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.Beginning:   //一位操作码
//                        if (Leftchars() > 0)
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.Start:   //一位操作码
//                        if (Textpos() != Textstart())
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.EndZ:    //一位操作码
//                        if (Rightchars() > 1 || Rightchars() == 1 && CharAt(Textpos()) != '\n')
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.End:     //一位操作码
//                        if (Rightchars() > 0)
//                            break;
//                        Advance();
//                        continue;

//                    case RegexCode.One:     //两位操作码
//                        if (Forwardchars() < 1 || Forwardcharnext() != (char)Operand(0))    //*
//                            break;

//                        Advance(1);
//                        continue;

//                    case RegexCode.Notone:  //两位操作码
//                        if (Forwardchars() < 1 || Forwardcharnext() == (char)Operand(0))    //*
//                            break;

//                        Advance(1);
//                        continue;

//                    case RegexCode.Set:     //两位操作码
//                        if (Forwardchars() < 1 || !RegexCharClass.CharInClass(Forwardcharnext(), runstrings[Operand(0)]))   //* CharInClass实现？
//                            break;

//                        Advance(1);
//                        continue;

//                    case RegexCode.Multi:       //两位操作码
//                        {
//                            if (!Stringmatch(runstrings[Operand(0)]))   //*
//                                break;

//                            Advance(1);
//                            continue;
//                        }

//                    case RegexCode.Ref:     //两位操作码
//                        {
//                            int capnum = Operand(0);

//                            if (IsMatched(capnum)) {
//                                if (!Refmatch(MatchIndex(capnum), MatchLength(capnum))) //* 判断后向引用是否相等
//                                    break;
//                            } else {
//                                if ((runregex.roptions & RegexOptions.ECMAScript) == 0)
//                                    break;
//                            }

//                            Advance(1);
//                            continue;
//                        }

//                    case RegexCode.Onerep:      //三位操作码
//                        {
//                            int c = Operand(1);     //获取重复次数

//                            if (Forwardchars() < c) //剩余字符数小于重复次数
//                                break;

//                            char ch = (char)Operand(0);   //获取重复的字符  

//                            while (c-- > 0)
//                                if (Forwardcharnext() != ch)    //*
//                                    goto BreakBackward;

//                            Advance(2);     //操作码向前两位
//                            continue;
//                        }

//                    case RegexCode.Notonerep:   //三位操作码
//                        {
//                            int c = Operand(1);     //获取重复次数

//                            if (Forwardchars() < c) //剩余字符数小于重复次数
//                                break;

//                            char ch = (char)Operand(0); //获取重复的字符

//                            while (c-- > 0)
//                                if (Forwardcharnext() == ch)    //*
//                                    goto BreakBackward;

//                            Advance(2);     //操作码向前两位
//                            continue;
//                        }

//                    case RegexCode.Setrep:  //三位操作码
//                        {
//                            int c = Operand(1);     //获取重复次数

//                            if (Forwardchars() < c)     //剩余字符数小于重复次数
//                                break;

//                            String set = runstrings[Operand(0)];    //获取集合的特殊表示字符串

//                            while (c-- > 0)
//                                if (!RegexCharClass.CharInClass(Forwardcharnext(), set))    //*
//                                    goto BreakBackward;

//                            Advance(2);     //操作码向前两位
//                            continue;
//                        }

//                    case RegexCode.Oneloop:     //三位操作码
//                        {
//                            int c = Operand(1);     //获取最大重复次数

//                            if (c > Forwardchars())
//                                c = Forwardchars();     //剩余字符数小于最大重复次数则将剩余字符数作为最大重复次数

//                            char ch = (char)Operand(0);     //获取重复的字符
//                            int i;

//                            for (i = c; i > 0; i--) {
//                                if (Forwardcharnext() != ch) {      //*
//                                    Backwardnext();     //输入字符的位置回退一位
//                                    break;
//                                }
//                            }

//                            if (c > i)
//                                TrackPush(c - i - 1, Textpos() - Bump());   //把长度-1，匹配位置-1，操作码依次压入track

//                            Advance(2); //操作码向前两位
//                            continue;
//                        }

//                    case RegexCode.Notoneloop:  //同oneloop，把比较字符相等改成不等即可
//                        {
//                            int c = Operand(1);

//                            if (c > Forwardchars())
//                                c = Forwardchars();

//                            char ch = (char)Operand(0);
//                            int i;

//                            for (i = c; i > 0; i--) {
//                                if (Forwardcharnext() == ch) {  //*
//                                    Backwardnext();
//                                    break;
//                                }
//                            }

//                            if (c > i)
//                                TrackPush(c - i - 1, Textpos() - Bump());

//                            Advance(2);
//                            continue;
//                        }

//                    case RegexCode.Setloop:     //三位操作码
//                        {
//                            int c = Operand(1);

//                            if (c > Forwardchars())
//                                c = Forwardchars();

//                            String set = runstrings[Operand(0)];
//                            int i;

//                            for (i = c; i > 0; i--) {
//                                if (!RegexCharClass.CharInClass(Forwardcharnext(), set)) {  //*
//                                    Backwardnext();
//                                    break;
//                                }
//                            }

//                            if (c > i)
//                                TrackPush(c - i - 1, Textpos() - Bump());

//                            Advance(2);
//                            continue;
//                        }

//                    case RegexCode.Oneloop | RegexCode.Back:
//                    case RegexCode.Notoneloop | RegexCode.Back:
//                        {
//                            TrackPop(2);
//                            int i   = TrackPeek();
//                            int pos = TrackPeek(1);

//                            Textto(pos);

//                            if (i > 0)
//                                TrackPush(i - 1, pos - Bump());

//                            Advance(2);
//                            continue;
//                        }

//                    case RegexCode.Setloop | RegexCode.Back:
//                        {
//                            TrackPop(2);
//                            int i   = TrackPeek();
//                            int pos = TrackPeek(1);

//                            Textto(pos);

//                            if (i > 0)
//                                TrackPush(i - 1, pos - Bump());

//                            Advance(2);
//                            continue;
//                        }

//                    case RegexCode.Onelazy:
//                    case RegexCode.Notonelazy:  //三位操作码
//                        {
//                            int c = Operand(1);

//                            if (c > Forwardchars())
//                                c = Forwardchars();

//                            if (c > 0)
//                                TrackPush(c - 1, Textpos());      //先不匹配 trackpush的作用？

//                            Advance(2);
//                            continue;
//                        }

//                    case RegexCode.Setlazy:     //三位操作码
//                        {
//                            int c = Operand(1);

//                            if (c > Forwardchars())
//                                c = Forwardchars();

//                            if (c > 0)
//                                TrackPush(c - 1, Textpos());    //不匹配

//                            Advance(2);
//                            continue;
//                        }

//                    case RegexCode.Onelazy | RegexCode.Back:
//                        {
//                            TrackPop(2);
//                            int pos = TrackPeek(1);
//                            Textto(pos);

//                            if (Forwardcharnext() != (char)Operand(0))
//                                break;

//                            int i = TrackPeek();

//                            if (i > 0)
//                                TrackPush(i - 1, pos + Bump());

//                            Advance(2);
//                            continue;
//                        }

//                    case RegexCode.Notonelazy | RegexCode.Back:
//                        {
//                            TrackPop(2);
//                            int pos = TrackPeek(1);
//                            Textto(pos);

//                            if (Forwardcharnext() == (char)Operand(0))
//                                break;

//                            int i = TrackPeek();

//                            if (i > 0)
//                                TrackPush(i - 1, pos + Bump());

//                            Advance(2);
//                            continue;
//                        }

//                    case RegexCode.Setlazy | RegexCode.Back:
//                        {
//                            TrackPop(2);
//                            int pos = TrackPeek(1);
//                            Textto(pos);

//                            if (!RegexCharClass.CharInClass(Forwardcharnext(), runstrings[Operand(0)]))
//                                break;

//                            int i = TrackPeek();

//                            if (i > 0)
//                                TrackPush(i - 1, pos + Bump());

//                            Advance(2);
//                            continue;
//                        }

//                    default:
//                        throw new NotImplementedException(SR.GetString(SR.UnimplementedState));
//                }

//                BreakBackward: 
//                ;

//                // "break Backward" comes here:
//                Backtrack();
//            }

//        }