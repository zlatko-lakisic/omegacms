#!/usr/bin/env bash
set -euo pipefail

root="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "${root}/Powershell/Import-TestRunEnv.sh"

import_test_run_env "$root"
assert_test_run_required_env \
  OMEGA_CF_STACK_NAME \
  OMEGA_CF_DOMAIN \
  OMEGA_CF_CONTAINER_VERSION \
  OMEGA_CF_CMS_VERSION \
  OMEGA_CF_PLUGINS_LAYER_VERSION \
  OMEGA_CF_OTHER_PLUGINS_ARN \
  OMEGA_CF_VPC_SECURITY_GROUP_ID \
  OMEGA_CF_VPC_SUBNET1_ID \
  OMEGA_CF_VPC_SUBNET2_ID \
  OMEGA_CF_ACM_CERTIFICATE_ARN \
  OMEGA_SMTP_USER \
  OMEGA_SMTP_PASSWORD \
  OMEGA_SMTP_HOST \
  OMEGA_SMTP_PORT \
  OMEGA_SMTP_USE_SSL \
  OMEGA_CMS_MYSQL_CONNECTION_STRING \
  OMEGA_CMS_UPLOADS_ACCESS_KEY_ID \
  OMEGA_CMS_UPLOADS_SECRET_ACCESS_KEY \
  OMEGA_AWS_REGION

"${root}/Powershell/Create.sh" \
  -stackName "${OMEGA_CF_STACK_NAME}" \
  -containerVersion "${OMEGA_CF_CONTAINER_VERSION}" \
  -cmsVersionParam "${OMEGA_CF_CMS_VERSION}" \
  -pluginsLayerVersion "${OMEGA_CF_PLUGINS_LAYER_VERSION}" \
  -otherPlugins "${OMEGA_CF_OTHER_PLUGINS_ARN}" \
  -vpcSecurityGroupId "${OMEGA_CF_VPC_SECURITY_GROUP_ID}" \
  -vpcSubnet1Id "${OMEGA_CF_VPC_SUBNET1_ID}" \
  -vpcSubnet2Id "${OMEGA_CF_VPC_SUBNET2_ID}" \
  -certificateArn "${OMEGA_CF_ACM_CERTIFICATE_ARN}" \
  -domain "${OMEGA_CF_DOMAIN}" \
  -emailUsername "${OMEGA_SMTP_USER}" \
  -emailPassword "${OMEGA_SMTP_PASSWORD}" \
  -emailHost "${OMEGA_SMTP_HOST}" \
  -emailPort "${OMEGA_SMTP_PORT}" \
  -emailSsl "${OMEGA_SMTP_USE_SSL}" \
  -connectionString "${OMEGA_CMS_MYSQL_CONNECTION_STRING}" \
  -uploadsAccessKey "${OMEGA_CMS_UPLOADS_ACCESS_KEY_ID}" \
  -uploadsSecretKey "${OMEGA_CMS_UPLOADS_SECRET_ACCESS_KEY}" \
  -regionName "${OMEGA_AWS_REGION}"
