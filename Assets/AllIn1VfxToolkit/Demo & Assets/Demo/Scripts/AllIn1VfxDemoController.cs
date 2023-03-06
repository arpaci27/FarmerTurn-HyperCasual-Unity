using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1VfxToolkit.Demo.Scripts
{
    public class AllIn1VfxDemoController : MonoBehaviour
    {
        [SerializeField] private int startingCollectionIndex, startingEffectIndex;
        
        [Space, Header("Demo Effects")]
        [SerializeField] private All1VfxDemoEffectCollection[] effectsToSpawnCollections;
        
        [Space, Header("Projectile References")]
        [Space, SerializeField] private GameObject projectileBasePrefab;
        [SerializeField] private GameObject projectileSceneSetupObject;
        [SerializeField] private Transform projectileSpawnPoint;
        
        [Space, Header("Demo Controller Input")]
        [SerializeField] private KeyCode playEffectKey = KeyCode.Q;
        [SerializeField] private KeyCode nextEffectKey = KeyCode.RightArrow;
        [SerializeField] private KeyCode nextEffectKeyAlt = KeyCode.D;
        [SerializeField] private KeyCode previousEffectKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode previousEffectKeyAlt = KeyCode.A;
        
        [Space, Header("UI and Other References")]
        [SerializeField] private Text currentEffectLabel;
        [SerializeField] private Button playEffectButton;
        [SerializeField] private GameObject playEffectInstructionsGo;
        [SerializeField] private Button nextEffectButton, previousEffectButton;
        [SerializeField] private Transform groundSpawnTransform, cameraPivotTransform;
        [SerializeField] private float camPivotHeightSmoothing;
        [SerializeField] private GameObject projectileEffectUI, normalEffectUI;
        

        private All1VfxDemoEffect currDemoEffect;
        private int currDemoCollectionIndex, currDemoEffectIndex, currentEffectPlays;
        private AllIn1DemoScaleTween currLabelTween, playButtTween, nextButtTween, prevButtTween;
        private float timeSinceEffectPlay;
        private AllIn1TimeControl allIn1TimeControl;

        private void Start()
        {
            projectileSceneSetupObject.SetActive(false);
            currDemoCollectionIndex = startingCollectionIndex;
            currDemoEffectIndex = startingEffectIndex;
            currLabelTween = currentEffectLabel.GetComponent<AllIn1DemoScaleTween>();
            playButtTween = playEffectButton.GetComponent<AllIn1DemoScaleTween>();
            nextButtTween = nextEffectButton.GetComponent<AllIn1DemoScaleTween>();
            prevButtTween = previousEffectButton.GetComponent<AllIn1DemoScaleTween>();
            allIn1TimeControl = gameObject.GetComponent<AllIn1TimeControl>();
            SetupAndInstantiateCurrentEffect();
        }

        private void Update()
        {
            if(currDemoEffect.canBePlayedAgain && Input.GetKeyDown(playEffectKey)) PlayCurrentEffect();
            if(Input.GetKeyDown(nextEffectKey) || Input.GetKeyDown(nextEffectKeyAlt)) ChangeCurrentEffect(1);
            else if(Input.GetKeyDown(previousEffectKey) || Input.GetKeyDown(previousEffectKeyAlt)) ChangeCurrentEffect(-1);

            if(currDemoEffect.spawnTouchingFloor) cameraPivotTransform.position = Vector3.Lerp(cameraPivotTransform.position, new Vector3(0f, 0.1f, 0f), Time.unscaledDeltaTime * camPivotHeightSmoothing);
            if(!currDemoEffect.spawnTouchingFloor) cameraPivotTransform.position = Vector3.Lerp(cameraPivotTransform.position, new Vector3(0f, 2f, 0f), Time.unscaledDeltaTime * camPivotHeightSmoothing);

            CooldownHandling();
        }

        private void CooldownHandling()
        {
            if(!currDemoEffect.canBePlayedAgain) return;
            timeSinceEffectPlay += Time.deltaTime;
            playEffectButton.interactable = currentEffectPlays < 1 || timeSinceEffectPlay >= currDemoEffect.cooldown;
        }

        public void PlayCurrentEffect(bool isAfterSetupAndInstantiateEffect = false)
        {
            if(currentEffectPlays > 0 && timeSinceEffectPlay < currDemoEffect.cooldown) return; //Return if on cooldown and not first play
                
            if(!isAfterSetupAndInstantiateEffect && Time.timeSinceLevelLoad > 0.1f) playButtTween.ScaleUpTween();
            
            if(!isAfterSetupAndInstantiateEffect && currDemoEffect.onlyOneAtATime) DestroyAllChildren();
            
            timeSinceEffectPlay = 0f;

            Transform tempTransform = null;
            if(currDemoEffect.isShootProjectile)
            {
                if(currDemoEffect.muzzleFlashPrefab != null)
                {
                    tempTransform = Instantiate(currDemoEffect.muzzleFlashPrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
                    tempTransform.localRotation = Quaternion.identity;
                    tempTransform.forward = projectileSpawnPoint.forward;
                    tempTransform.parent = transform;
                    tempTransform.localScale *= currDemoEffect.scaleMultiplier;
                }
                Transform projectileBase = Instantiate(projectileBasePrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
                projectileBase.forward = projectileSpawnPoint.forward;
                projectileBase.parent = transform;
                projectileBase.localRotation = Quaternion.identity;
                
                tempTransform = Instantiate(currDemoEffect.projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
                tempTransform.localRotation = Quaternion.identity;
                tempTransform.forward = projectileSpawnPoint.forward;
                tempTransform.parent = projectileBase;
                AllIn1DemoProjectile tempProjectileInstance = projectileBase.GetComponent<AllIn1DemoProjectile>();
                tempProjectileInstance.Initialize(transform, projectileSpawnPoint.forward, currDemoEffect.projectileSpeed, currDemoEffect.impactPrefab, currDemoEffect.scaleMultiplier);
                if(currDemoEffect.doCameraShake) tempProjectileInstance.AddScreenShakeOnImpact(currDemoEffect.projectileImpactShakeAmount);
            }
            else
            {
                tempTransform = Instantiate(currDemoEffect.effectPrefab, transform).transform;
                if(!currDemoEffect.spawnTouchingFloor) tempTransform.localPosition = Vector3.zero;
                else tempTransform.position = groundSpawnTransform.position;
                tempTransform.localRotation = currDemoEffect.effectPrefab.transform.rotation;
                if(currDemoEffect.canBePlayedAgain && currDemoEffect.randomSpreadRadius > 0f && currentEffectPlays > 0)
                {
                    tempTransform.position += new Vector3(Random.Range(-currDemoEffect.randomSpreadRadius, currDemoEffect.randomSpreadRadius), 0f,
                        Random.Range(-currDemoEffect.randomSpreadRadius, currDemoEffect.randomSpreadRadius));
                }
            }
            tempTransform.localScale *= currDemoEffect.scaleMultiplier;
            tempTransform.position += currDemoEffect.positionOffset;
            
            if(!isAfterSetupAndInstantiateEffect && currDemoEffect.doCameraShake) AllIn1Shaker.i.DoCameraShake(currDemoEffect.mainEffectShakeAmount);

            currentEffectPlays++;
        }

        public void ChangeCurrentEffect(int changeAmount)
        {
            if(changeAmount < 0) prevButtTween.ScaleUpTween();
            else if(changeAmount > 0) nextButtTween.ScaleUpTween();
            StartCoroutine(CurrentEffectLabelTweenEffectCR());
            currDemoEffectIndex += changeAmount;
            SetupAndInstantiateCurrentEffect();
            allIn1TimeControl.CurrentEffectChanged();
        }

        private void SetupAndInstantiateCurrentEffect()
        {
            DestroyAllChildren();
            currentEffectPlays = 0;
            ComputeValidEffectAndCollectionIndex();
            currDemoEffect = effectsToSpawnCollections[currDemoCollectionIndex].demoEffectCollection[currDemoEffectIndex];
            projectileSceneSetupObject.SetActive(currDemoEffect.isShootProjectile);
            projectileEffectUI.SetActive(currDemoEffect.isShootProjectile);
            normalEffectUI.SetActive(!currDemoEffect.isShootProjectile);
            currentEffectLabel.text = currDemoEffect.isShootProjectile ? currDemoEffect.projectilePrefab.name : currDemoEffect.effectPrefab.name;
            playEffectButton.gameObject.SetActive(currDemoEffect.canBePlayedAgain);
            playEffectInstructionsGo.SetActive(currDemoEffect.canBePlayedAgain);
            PlayCurrentEffect(true);
        }

        private void ComputeValidEffectAndCollectionIndex()
        {
            int demoEffectOperation = 0; // 0 means no operation, 1 means assign first collection effect, 2 means last collection effect
            if(currDemoEffectIndex < 0)
            {
                currDemoCollectionIndex--;
                demoEffectOperation = 2;
            }
            else if(currDemoEffectIndex >= effectsToSpawnCollections[currDemoCollectionIndex].demoEffectCollection.Length)
            {
                currDemoCollectionIndex++;
                demoEffectOperation = 1;
            }

            if(currDemoCollectionIndex < 0)
            {
                currDemoCollectionIndex = effectsToSpawnCollections.Length - 1;
                demoEffectOperation = 2;
            }
            else if(currDemoCollectionIndex >= effectsToSpawnCollections.Length)
            {
                currDemoCollectionIndex = 0;
                demoEffectOperation = 1;
            }

            if(demoEffectOperation > 0)
            {
                if(demoEffectOperation == 1) currDemoEffectIndex = 0;
                else if(demoEffectOperation == 2) currDemoEffectIndex = effectsToSpawnCollections[currDemoCollectionIndex].demoEffectCollection.Length - 1;
            }
        }

        private IEnumerator CurrentEffectLabelTweenEffectCR()
        {
            //This mess of a function prevents the Best Fit on currentEffectLabel to scale in a weird way during 1 frame
            Color startColor = currentEffectLabel.color;
            currLabelTween.ScaleDownTween();
            currentEffectLabel.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
            yield return null;
            currentEffectLabel.color = new Color(startColor.r, startColor.g, startColor.b, 1f);
        }

        private void DestroyAllChildren()
        {
            foreach(Transform child in transform) Destroy(child.gameObject);
        }
    }
}