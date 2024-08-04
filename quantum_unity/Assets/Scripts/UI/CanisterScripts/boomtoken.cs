using System;
using System.Collections;
using System.Linq;
using System.Numerics;
using Candid;
using CanisterPK.BoomToken.Models;
using EdjCase.ICP.Candid.Models;
using TMPro;
using UnityEngine;

namespace Cosmicrafts.Data
{
    public class boomtoken : MonoBehaviour
    {
        public TMP_Text balanceText;
        public TMP_InputField principalInputField;
        public TMP_InputField tokenAmountInputField;
        public TMP_Text transferStatusText;
        public Animator BoomPanel;
        public BigInteger CurrentBalance { get; private set; } = BigInteger.Zero;

        private const int DECIMAL_PLACES = 6;

        void Start()
        {
            BoomPanel = GameObject.Find("BoomPanel").GetComponent<Animator>();
            FetchBalance();
        }

        private void OnEnable()
        {
            BalanceManager.OnBalanceUpdateNeeded += FetchBalance;
        }

        private void OnDisable()
        {
            BalanceManager.OnBalanceUpdateNeeded -= FetchBalance;
        }

        public async void FetchBalance()
        {
            try
            {
                PlayerData playerData = await AsyncDataManager.LoadPlayerDataAsync();
                string principalId = playerData.PrincipalId;
                Debug.Log($"Fetching balance for Principal ID: {principalId}");

                var account = new CanisterPK.BoomToken.Models.Account(Principal.FromText(principalId), new Account.SubaccountInfo());
                var balance = await CandidApiManager.Instance.boomToken.Icrc1BalanceOf(account);

                // Trigger the balance animation with the new balance
                AnimateBalanceUpdate(balance);

                if (BoomPanel == null)
                {
                    Debug.LogError("Panel Animator is not assigned.");
                }
                else
                {
                    BoomPanel.Play("TokenPanelRefresh", -1, 0f);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error fetching balance: {ex.Message}");
            }
        }

        private string FormatBalance(UnboundedUInt balance)
        {
            return balance.ToString();
        }

        private BigInteger ConvertToBigInteger(decimal value)
        {
            return BigInteger.Parse(value.ToString());
        }

        // Method to start the balance update animation
        private void AnimateBalanceUpdate(UnboundedUInt newBalance)
        {
            // Assuming you have a method to convert UnboundedUInt to BigInteger or decimal for comparison
            BigInteger newBalanceBigInt = newBalance.ToBigInteger();
            CurrentBalance = newBalanceBigInt;
            StartCoroutine(IncrementBalanceAnimation(newBalanceBigInt));
        }

        // Coroutine for animating the balance update
        private IEnumerator IncrementBalanceAnimation(BigInteger targetBalance)
        {
            float duration = 0.5f;
            BigInteger currentBalance;

            if (!BigInteger.TryParse(SanitizeText(balanceText.text), out currentBalance))
            {
                Debug.Log("Current balance is not a valid number, defaulting to zero.");
                currentBalance = BigInteger.Zero;
            }

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / duration);

                BigInteger progressBalance = currentBalance + (targetBalance - currentBalance) * new BigInteger((long)(progress * 100000)) / new BigInteger(100000);

                balanceText.text = progressBalance.ToString();
                yield return null;
            }

            balanceText.text = targetBalance.ToString();
        }

        private string SanitizeText(string text)
        {
            return new string(text.Where(char.IsDigit).ToArray());
        }
    }
}