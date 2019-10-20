using System;
using System.Collections.Generic;
using CADPadDB;
using CADPadDB.CADEntity;

namespace CADPadServices.ESelection
{
    public class EntityRSMgr
    {
        /// <summary>
        /// Singleton
        /// </summary>
        private static EntityRSMgr _instance = null;
        public static EntityRSMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EntityRSMgr();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Dictionary<Type, EntityRS> _type2EntityRS = new Dictionary<Type, EntityRS>();

        private EntityRSMgr()
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterEntityRS(typeof(Line), new LineRS());
            //RegisterEntityRS(typeof(Xline), new XlineRS());
            //RegisterEntityRS(typeof(Ray), new RayRS());
            //RegisterEntityRS(typeof(Polyline), new PolylineRS());
            //RegisterEntityRS(typeof(Circle), new CircleRS());
            //RegisterEntityRS(typeof(Arc), new ArcRS());
            //RegisterEntityRS(typeof(Text), new TextRS());
        }

        private void RegisterEntityRS(Type type, EntityRS entityRS)
        {
            _type2EntityRS[type] = entityRS;
        }

        /// <summary>
        /// Select
        /// </summary>
        public bool Cross(Bounding selectBound, Entity entity)
        {
            if (_type2EntityRS.ContainsKey(entity.GetType()))
            {
                return _type2EntityRS[entity.GetType()].Cross(selectBound, entity);
            }

            return false;
        }

        public bool Window(Bounding selectBound, Entity entity)
        {
            if (_type2EntityRS.ContainsKey(entity.GetType()))
            {
                return _type2EntityRS[entity.GetType()].Window(selectBound, entity);
            }

            return false;
        }
    }
}