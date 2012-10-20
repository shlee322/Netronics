namespace Netronics.DB.Where
{
    class ModelWhere : Where
    {
        private Model _model;

        public ModelWhere(Model model)
        {
            _model = model;
        }

        public Model GetModel()
        {
            return _model;
        }
    }
}
