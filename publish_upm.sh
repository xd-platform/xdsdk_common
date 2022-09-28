#!/bin/sh
# 获取当前分支
currentBranch=$(git symbolic-ref --short -q HEAD)

upmModule=("xdsdk-account-upm" "xdsdk-common-upm" "xdsdk-payment-upm" "xdsdk-oversea-upm" "xdsdk-mainland-upm")
module=("Account" "Common" "Payment" "Oversea" "Mainland")
githubRepoName=("xdsdk_account" "xdsdk_common" "xdsdk_payment" "xdsdk_oversea" "xdsdk_mainland")

#单发模块
#upmModule=("xd-sdk-payment-upm")
#module=("Payment")
#githubRepoName=("xdsdk_payment")

tag=$1
#是否正式发布，
publish2Release=$2

# 发布 UPM 脚本
publishUPM() {
    git tag -d $(git tag)
    
    git branch -D github_upm
    
    git subtree split --prefix=Assets/XD/SDK/"$1" --branch github_upm
    
    git remote rm "$2"
    
    if [ $publish2Release = true ]; then
        echo "start push $2 to git@github.com:xd-platform/$4.git"
        git remote add "$2" git@github.com:xd-platform/"$4".git
    else
        echo "start push $2 to git@github.com:luckisnow/$4.git"  
        git remote add "$2" git@github.com:luckisnow/"$4".git
    fi;
    
    git checkout github_upm --force
    
    git tag "$3"
    
    git fetch --unshallow github_upm
    
    git push "$2" github_upm --force --tags
    
    git checkout "$currentBranch" --force
        
}
for ((i=0;i<${#upmModule[@]};i++)); do
    publishUPM "${module[$i]}" "${upmModule[$i]}" "$tag" "${githubRepoName[$i]}" 
done