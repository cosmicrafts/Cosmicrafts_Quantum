namespace CosmicraftsSP
{
    using UnityEngine;

    [CreateAssetMenu(fileName = ("Nueva Hechizo"), menuName = ("Crear Nuevo Hechizo"))]
    public class SpellsDataBase : ScriptableObject
    {
        #region DataBase

        [Tooltip("Prefab asociado")]
        [Header("Prefab")]
        [SerializeField]
        protected GameObject Prefab;

        [Tooltip("Informa de la Nombre del hechizo")]
        [Header("Nombre del hechizo")]
        [SerializeField]
        protected string Name;

        [Tooltip("ID local del hechizo")]
        [Header("ID Local")]
        [SerializeField]
        protected int LocalID;

        [Tooltip("Faccion del hechizo")]
        [Header("Faccion")]
        [SerializeField]
        protected Factions Faction;

        [Tooltip("Costo de energia del hechizo")]
        [Header("Asignar el costo del hechizo")]
        [SerializeField]
        [Range(1, 9999)]
        protected int EnergyCost;

        #endregion

        #region Variables de Lectura

        public GameObject prefab => Prefab;
        public string cardName => Name;
        public int localId => LocalID;
        public int faction => (int)Faction;
        public int cost => EnergyCost;

        public NFTsSpell ToNFTCard()
        {
            NFTsSpell nFTsCard = new NFTsSpell()
            {
                EnergyCost = cost,
                Faction = faction,
                EntType = (int)NFTClass.Skill,
                LocalID = localId,
                TypePrefix = NFTsCollection.NFTsPrefix[(int)NFTClass.Skill],
                FactionPrefix = NFTsCollection.NFTsFactionsPrefixs[(Factions)faction],
            };
            nFTsCard.IconSprite = ResourcesServices.LoadCardIcon(nFTsCard.KeyId);
            return nFTsCard;
        }

        #endregion
    }
}
