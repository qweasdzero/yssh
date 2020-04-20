## GameFramework_UGUIBinding

### 每个页面预制体 需要绑定 root model view 组件

### 脚本分为数据层 和 逻辑处理层

        public class MenuFormModel : UGuiFormModel<MenuForm,MenuFormModel>
        {
        
            /** 绑定的数据类型*/
            #region TextProperty
    
            private readonly Property<string> _privateTextProperty = new Property<string>();
    
            public Property<string> TextProperty
            {
                get { return _privateTextProperty; }
            }
    
            public string Text
            {
                get { return _privateTextProperty.GetValue(); }
                set { _privateTextProperty.SetValue(value); }
            }
    
            #endregion
        }
    
        public class MenuForm : UGuiFormPage<MenuForm,MenuFormModel>
        {
            protected override void OnOpen(object userData)
            {
                base.OnOpen(userData);
                /** 为绑定的数赋值*/
                Model.Text = "ugui_textBinding";
            }
        }
        
### rider 生成模板

1.  model

        #region $name$Property
        private readonly Property<$type$> _private$name$Property = new Property<$type$>();
        
        public Property<$type$> $name$Property
        {
            get{return _private$name$Property;}
        }
        public $type$ $name$
        {
            get { return _private$name$Property.GetValue(); }
            set { _private$name$Property.SetValue(value); }
        }
        #endregion
        $SELECTION$$END$

2.  modelcollection

        #region $name$Collect
        private readonly Collection<$type$> _private$name$Collection = new Collection<$type$>();
        
        public Collection<$type$> $name$
        {
            get{return _private$name$Collection;}
        }
        #endregion
        $SELECTION$$END$